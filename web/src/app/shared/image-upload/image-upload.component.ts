import { Component, Input } from '@angular/core';

export class ImageSnippet {
  src: string = '';
  pending: boolean = false;
  status: string = 'init';

  constructor(public file: File) {}
}

@Component({
  selector: 'image-upload',
  templateUrl: 'image-upload.component.html',
  styleUrls: ['image-upload.component.css'],
})
export class ImageUploadComponent {
  @Input() idEvento: number;

  selectedFile: ImageSnippet;
  imagemJaCarregada: string = '';

  constructor() {}

  ngInit(): void {
    const imageName = this.idEvento + '.png';

    this.imagemJaCarregada =
      'C:/xampp/htdocs/assets/Images/Eventos/' + imageName;
  }

  private onSuccess() {
    this.selectedFile.pending = false;
    this.selectedFile.status = 'ok';
  }

  protected processFile(imageInput: any) {
    const file: File = imageInput.files[0];
    const reader = new FileReader();

    reader.addEventListener('load', (event: any) => {
      this.selectedFile = new ImageSnippet(file);

      this.selectedFile.src = event.target.result;
      this.selectedFile.pending = true;
      this.onSuccess();
    });

    reader.readAsDataURL(file);
  }

  getImagem(): File {
    return this.selectedFile.file;
  }
}
