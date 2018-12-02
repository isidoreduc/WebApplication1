import { Component, Inject, OnInit, Input } from "@angular/core";
import { QuizService } from "../../services/quiz.service";

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
  errorMsg: string;

  constructor(private quizService: QuizService) {
    
  }

  ngOnInit(): void {
    switch (this.class) {
      case "latest":
      default:
        this.title = "Latest Quizzes";
        
        break;
      case "byTitle":
        this.title = "Quizzes by Title";
        
        break;
      case "random":
        this.title = "Random Quizzes";
        
        break;
    }
    //this.title = "Latest Quizzes";
    this.quizService.getQuizzes().subscribe(
      quizzes => this.quizzes = quizzes,
      error => this.errorMsg = <any>error);
  }

  onSelect(quiz: Quiz) {
    this.selectedQuiz = quiz;
    console.log("quiz with Id "
      + this.selectedQuiz.Id
      + " has been selected.");
  }


}
