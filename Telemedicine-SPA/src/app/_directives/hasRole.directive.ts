import { Directive, Input, ViewContainerRef, TemplateRef, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Directive({
  selector: '[appHasRole]' // call as *appHasRole
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[];
  isVisible = false;

  // A container for 2 different views: component or template
  // Used to view templates in particular here
  constructor(
    private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>,
    private authService: AuthService) { }

    ngOnInit() {
      const userRoles = this.authService.decodedToken.role as Array<string>;
      // if no roles clear the viewContainerRef
      if (!userRoles) {
        this.viewContainerRef.clear(); // if user is not a member of a role, we show nothing
      }

      // if user has role need then render the element
      if (this.authService.roleMatch(this.appHasRole)) {
        if (!this.isVisible) {
          this.isVisible = true;
          this.viewContainerRef.createEmbeddedView(this.templateRef);
          // this.templateRef refers to element we're adding this directive too
        }
      } else {
        this.isVisible = false;
        this.viewContainerRef.clear();
      }
    }

}
