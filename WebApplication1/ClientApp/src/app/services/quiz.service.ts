import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Input } from "@angular/core";
import { Observable } from "rxjs";



@Injectable()
export class QuizService {
  quizUrl: string = "";
  @Input() class: string;

  constructor(private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string) {
    this.quizUrl = baseUrl + "api/quiz/";
    switch (this.class) {
      case "latest":
      default:
        this.quizUrl += "Latest/5";
        break;
      case "byTitle":

        this.quizUrl += "ByTitle/";
        break;
      case "random":

        this.quizUrl += "Random/";
        break;

    }

  }
  getQuizzes(): Observable<Quiz[]> {
    return this.http.get<Quiz[]>(this.quizUrl);
  }
}
