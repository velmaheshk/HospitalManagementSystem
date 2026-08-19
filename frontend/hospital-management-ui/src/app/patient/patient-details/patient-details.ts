import { CommonModule } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  OnInit
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PatientService } from '../../Service/patientservice';


@Component({
  selector: 'app-patient-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './patient-details.html',
  styleUrls: ['./patient-details.scss']
})
export class PatientDetails implements OnInit {

  patient: any = null;

  loading = true;

  constructor(
    private route: ActivatedRoute,
    private patientService: PatientService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

    const id = Number(
      this.route.snapshot.paramMap.get('id')
    );

    console.log('DETAILS ID =', id);

    if (!id) {
      this.loading = false;
      return;
    }

    this.patientService.getPatientById(id).subscribe({

      next: (response: any) => {

        console.log('DETAILS RESPONSE =', response);

        this.patient = response;

        console.log(
          'PATIENT ASSIGNED =',
          this.patient
        );

        this.loading = false;

        // FORCE ANGULAR UI UPDATE
        this.cdr.detectChanges();

        console.log('CHANGE DETECTION DONE');

      },

      error: (error) => {

        console.error(
          'DETAILS API ERROR =',
          error
        );

        this.loading = false;

        this.cdr.detectChanges();

      }

    });
  }
}