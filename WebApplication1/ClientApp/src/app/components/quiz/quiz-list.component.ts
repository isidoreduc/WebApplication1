import { Component, Inject, OnInit, Input } from "@angular/core";
import { QuizService } from "../../services/quiz.service";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "quiz-list",
  templateUrl: './quiz-list.component.html',
  styleUrls: ['./quiz-list.component.css']
})


export class QuizListComponent implements OnInit{
  @Input() class: string;
  title: string;
  selectedQuiz: Quiz;
  quizzes: Quiz[];
  //http: HttpClient;
  //baseUrl: string;

  // dependency injection of the HttpClient to connect to api and of the base url(localhost:44349, in this instance)
  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) { // an explicit access modifier (private, here) makes parameters available in the whole class not just in the constructor, no need to create instances in order to access the parameters outside the constructor
    //this.http = http;
    //this.baseUrl = baseUrl;

  }

  // using different classes of the same selector to get 3 different api calls in the same component
  ngOnInit(): void {
    console.log("QuizListComponent " +
      " instantiated with the following class: "
      + this.class);

    var url = this.baseUrl + "api/quiz/";
    switch (this.class) {
      case "latest":
      default:
        this.title = "Latest Quizzes";
        url += "Latest/10";
        break;
      case "byTitle":
        this.title = "Quizzes by Title";
        url += "ByTitle/";
        break;
      case "random":
        this.title = "Random Quizzes";
        url += "Random/";
        break;
    }
    // observable pattern
    this.http.get<Quiz[]>(url).subscribe(result =>
      this.quizzes = result,
      error => console.error(error));
  }

  onSelect(quiz: Quiz) {
    this.selectedQuiz = quiz;
    console.log("quiz with Id "
      + this.selectedQuiz.Id
      + " has been selected.");
  }
}
