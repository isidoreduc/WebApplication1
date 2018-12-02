import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Input, OnInit } from "@angular/core";
import { Observable } from "rxjs";

//

@Injectable()
export class QuizService  {
    
    
  quizUrl: string = "";
  @Input() class: string;
  http1: HttpClient;
  baseUrl1: string;

  constructor(private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string) {
    this.http1 = http;
    this.baseUrl1 = baseUrl;

    
    

  }

 

  getQuizzes(): Observable<Quiz[]> {
    this.quizUrl = this.baseUrl1 + "api/quiz/";
    //switch (this.class) {
    //  case "latest":

    //    this.quizUrl += "Latest/10";
    //    break;
    //  case "byTitle":

    //    this.quizUrl += "ByTitle/";
    //    break;
    //  case "random":
    //  //default:
    //    this.quizUrl += "Random/";
    //    break;
    //}
    //if (this.class == "latest")
    //  this.quizUrl += "Latest/10";
    return this.http.get<Quiz[]>(this.quizUrl + "Latest/10");
  }
}
