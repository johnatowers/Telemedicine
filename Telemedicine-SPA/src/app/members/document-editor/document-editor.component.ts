import { Component, OnInit, Input} from '@angular/core';
import { Document} from '../../_models/Document';
import {FileUploader} from 'ng2-file-upload';
import {environment} from '../../../environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-document-editor',
  templateUrl: './document-editor.component.html',
  styleUrls: ['./document-editor.component.css']
})
export class DocumentEditorComponent implements OnInit {
  @Input() documents: Document[];
  user: User;
  // @Output() getMemberPhotoChange = new EventEmitter<string>();

  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  // currentMain: Photo;

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService,
              private route: ActivatedRoute ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      // url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/documents',
      url: this.baseUrl + 'users/' + this.user.id + '/documents',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image', 'video'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 20 * 1024 * 1024
    });

    this.uploader.onErrorItem = (item, response, status, headers) => {
      this.alertify.error(response);
    };

    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
        if (response) {
          const res: Document = JSON.parse(response);
          const document = {
            id: res.id,
            url: res.url,
            dateAdded: res.dateAdded,
            description: res.description,
            isMain: res.isMain
          };
          this.documents.push(document);
        }
      };
   }

  deleteDocument(id: number) {
    this.alertify.confirm('Are you sure you want to delete this document?', () => {
      this.userService.deleteDocument(this.authService.decodedToken.nameid, id).subscribe(() => {
        this.documents.splice(this.documents.findIndex(p => p.id === id), 1);
        this.alertify.success('Document has been deleted');
      }, error => {
        this.alertify.error('Failed to delete document');
      });
    });
  }

  // setMainPhoto(photo: Photo) {
  //   this.userService.setMainPhoto(this.authService.decodedToken.nameid, photo.id).subscribe(() => {
  //     this.currentMain = this.photos.filter(p => p.isMain === true)[0];
  //     this.currentMain.isMain = false;
  //     photo.isMain = true;
  //     this.getMemberPhotoChange.emit(photo.url);
  //   }, error => {
  //     this.alertify.error(error);

  //   });
}
