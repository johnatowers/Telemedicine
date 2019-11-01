export interface Message {
    // properties we return in MessageToReturnDto
    id: number;
    senderId: number;
    senderUserName: string;
    // senderPhotoUrl: string;
    recipientId: number;
    recipientUserName: string;
    // recipientPhotoUrl: string;
    content: string;
    isRead: boolean;
    dateRead: Date;
    messageSent: Date;
}
