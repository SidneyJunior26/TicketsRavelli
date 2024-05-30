import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DadosMedicosService as dadosMedicosService } from 'src/app/core/RegistrosMedicos/dados-medicos.service';
import { DadosMedicos } from 'src/app/shared/models/dadosMedicos';

export interface DialogData {
  atletaId: string;
}

@Component({
  selector: 'app-dados-medicos',
  templateUrl: './dados-medicos.component.html',
  styleUrls: ['./dados-medicos.component.css'],
})
export class DadosMedicosComponent implements OnInit {
  constructor(
    private dadosMedicosService: dadosMedicosService,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  dadosMedicosControl = this.formBuilder.group({
    plano: new FormControl(false, Validators.required),
    planoEmpresa: new FormControl(''),
    planoTipo: new FormControl(''),
    pressaoalta: new FormControl(false, Validators.required),
    desmaio: new FormControl(false, Validators.required),
    cardiaco: new FormControl(false, Validators.required),
    diabetes: new FormControl(false, Validators.required),
    asma: new FormControl(false, Validators.required),
    alergia: new FormControl(false, Validators.required),
    alergiaQual: new FormControl(''),
    cirurgia: new FormControl(false, Validators.required),
    cirurgiaQual: new FormControl(''),
    medicacao: new FormControl(false, Validators.required),
    medicacaoQual: new FormControl(''),
    medicacaoTempo: new FormControl(''),
    malestar: new FormControl(false, Validators.required),
    malestarQual: new FormControl(''),
    acompanhamento: new FormControl(false, Validators.required),
    acompanhamentoQual: new FormControl(''),
    outros: new FormControl(''),
  });

  novoRegistro = false;
  dadosMedicos: DadosMedicos;

  ngOnInit(): void {
    this.dadosMedicosService
      .consultarDadosMedicosPorAtletaId(this.data.atletaId)
      .subscribe(
        (dadosMedicos) => {
          this.dadosMedicos = dadosMedicos;

          this.carregarControl();
        },
        (error) => {
          if (error.status == 404) this.novoRegistro = true;
        }
      );
  }

  private carregarControl() {
    for (const key in this.dadosMedicos) {
      if (this.dadosMedicosControl.get(key)) {
        this.dadosMedicosControl.get(key)?.setValue(this.dadosMedicos[key]);
      }
    }
    this.setValidators();
  }

  protected setValidators() {
    if (this.dadosMedicosControl.get('plano')!.value! == true) {
      this.dadosMedicosControl
        .get('planoEmpresa')!
        .setValidators(Validators.required);
      this.dadosMedicosControl
        .get('planoTipo')!
        .setValidators(Validators.required);
    } else {
      this.dadosMedicosControl.get('planoEmpresa')!.clearValidators();
      this.dadosMedicosControl.get('planoTipo')!.clearValidators();
    }

    if (this.dadosMedicosControl.get('cirurgia')?.value == true) {
      this.dadosMedicosControl
        .get('cirurgiaQual')!
        .setValidators(Validators.required);
    } else {
      this.dadosMedicosControl.get('cirurgiaQual')!.clearValidators();
    }

    if (this.dadosMedicosControl.get('medicacao')?.value == true) {
      this.dadosMedicosControl
        .get('medicacaoQual')!
        .setValidators(Validators.required);
      this.dadosMedicosControl
        .get('medicacaoTempo')!
        .setValidators(Validators.required);
    } else {
      this.dadosMedicosControl.get('medicacaoQual')!.clearValidators();
      this.dadosMedicosControl.get('medicacaoTempo')!.clearValidators();
    }

    if (this.dadosMedicosControl.get('malestar')?.value == true) {
      this.dadosMedicosControl
        .get('malestarQual')!
        .setValidators(Validators.required);
    } else {
      this.dadosMedicosControl.get('malestarQual')!.clearValidators();
    }

    if (this.dadosMedicosControl.get('acompanhamento')?.value == true) {
      this.dadosMedicosControl
        .get('acompanhamentoQual')!
        .setValidators(Validators.required);
    } else {
      this.dadosMedicosControl.get('acompanhamentoQual')!.clearValidators();
    }

    if (this.dadosMedicosControl.get('alergia')?.value == true) {
      this.dadosMedicosControl
        .get('alergiaQual')!
        .setValidators(Validators.required);
    } else {
      this.dadosMedicosControl.get('alergiaQual')!.clearValidators();
    }

    this.dadosMedicosControl.get('planoEmpresa')!.updateValueAndValidity();
    this.dadosMedicosControl.get('pressaoalta')!.updateValueAndValidity();
    this.dadosMedicosControl.get('planoTipo')!.updateValueAndValidity();
    this.dadosMedicosControl.get('cirurgiaQual')!.updateValueAndValidity();
    this.dadosMedicosControl.get('medicacaoQual')!.updateValueAndValidity();
    this.dadosMedicosControl.get('medicacaoTempo')!.updateValueAndValidity();
    this.dadosMedicosControl.get('malestarQual')!.updateValueAndValidity();
    this.dadosMedicosControl
      .get('acompanhamentoQual')!
      .updateValueAndValidity();
    this.dadosMedicosControl.get('alergiaQual')!.updateValueAndValidity();
  }

  protected salvarDadosMedicos() {}
}
