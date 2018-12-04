import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { QuizListComponent } from './components/quiz/quiz-list.component';
import { QuizService } from './services/quiz.service';
import { QuizComponent } from './components/quiz/quiz.component';
import { AboutComponent } from './components/about/about.component';
import { LoginComponent } from './components/login/login.component';
import { PageNotFoundComponent } from './components/pagenotfound/pagenotfound.component';
import { QuizEditComponent } from './components/quiz/quiz-edit.component';


@NgModule({
  declarations: [
    AppComponent, NavMenuComponent, HomeComponent, QuizListComponent,
    QuizComponent, AboutComponent, LoginComponent, PageNotFoundComponent,
    QuizEditComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: "quiz/create", component: QuizEditComponent }, // needs to be before the catch all route
      { path: 'quiz/edit/:id', component: QuizEditComponent },
      { path: 'quiz/:id', component: QuizComponent }, // catch all route for quiz
      { path: 'about', component: AboutComponent },
      { path: 'login', component: LoginComponent },
      { path: '**', component: PageNotFoundComponent } // global fallback, should be last or the other after it will never execute
    ])
  ],
  providers: [QuizService],
  bootstrap: [AppComponent]
})
export class AppModule { }
