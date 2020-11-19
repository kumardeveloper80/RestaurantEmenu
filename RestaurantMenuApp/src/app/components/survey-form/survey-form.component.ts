import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { SurveyFormService } from '../survey-form/survey-form.service';

enum HTMLELEMENT {
  TEXT = 1,
  CHECKBOXLIST = 2,
  RADIOBUTTONLIST = 3,
  DATE = 4,
  DROPDOWN = 5,
  TEXTBOX = 6,
  SLIDER = 7
}

enum QTYPEVALUE {
  type_Rating = 1,
  type_YesNo = 2,
  type_VisitFreq = 3
}

@Component({
  selector: 'app-survey-form',
  templateUrl: './survey-form.component.html',
  styleUrls: ['./survey-form.component.css']
})
export class SurveyFormComponent implements OnInit {

  surveyForm: any;
  HTMLELEMENT: typeof HTMLELEMENT = HTMLELEMENT;
  QTYPEVALUE: typeof QTYPEVALUE = QTYPEVALUE;
  @ViewChild('fillUpForm') fillUpForm: NgForm;

  constructor(
    private _route: ActivatedRoute,
    private _surveyFormService: SurveyFormService) { }

  ngOnInit(): void {
    this.getSurveyForm();
  }

  getSurveyForm() {
    const storeid = this._route.snapshot.paramMap.get('id');
    this._surveyFormService.getSurveyForm(storeid).subscribe((res) => {
      if (res["status"] === 1) {
        this.surveyForm = res["dataenum"];
      } else {
        alert(res["message"])
      }
    })
  }

  onSubmit() {
    this.surveyForm.questionMaster.forEach(element => {
      element.questions.forEach(que => {
        if (que.userAnswer !== null)
          que.userAnswer = que.userAnswer.toString()
      });
    });

    this._surveyFormService.saveSurveyForm(this.surveyForm).subscribe((res) => {
      if (res["status"] === 1) {
        alert(res["message"])
        location.reload();
      } else {
        alert(res["message"])
      }
    })
  }
}
