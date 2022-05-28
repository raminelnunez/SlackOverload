# SlackOverload

# StackOverload
* A StackOverflow clone using an ASP.NET MVC framework with an SQL Database complete with Seed Data 
* Completed solo for the final project of my Adv Database and ORM Concepts (Backend) SD-330 course at MITT 

## Assignment Requirements
Create a Q&A (Question and Answer) website similar to StackOverflow, where users can ask and answer questions.

Use EF Code First approach, which includes creating Users accounts and Roles. Those users can log in/log off.

The system should include the following:
* A Main Page which contains a list of "Recent Questions." This page displays the ten most recent questions, and a means of navigating between these pages (e.g. page 1 with questions 1-10, page 2 with questions 11-20, etc), also known as Pagination. 
  * This list should show the Title of the question, the number of Answers that have been provided for that question, the name of the Question's User, and the appropriate links as describe below.
  * An additional button on the page should allow the user to switch the sorting of this page from Date to Most Answered.
* Users should be able to post a new question, from a link in the main page. This page should provide separate fields for the question Title and the Body of the question, which should use a full text editor (not a single-line editor). A rich text editor would be even better!
* Users should be able to answer a question by clicking the "Answer this Question" link next to each question.
* Each question has a list of tags provided beneath them. When users click on a tag, they should be directed to a page containing all the questions that possess this tag (e.g. "Software", "Hardware", "Pets", etc.)
* Each Question has a Detail page that can be visited via a link on the Question List. It shows the Title of the question, the Body, the Tags, then a list of Answers. Users can also Comment on questions and answers.
* Each question and answer can have a number of votes, where users can either upvote or downvote them. Users can’t vote on their own questions and answers. Users cannot vote more than one time on one item; they can only switch between an up-or-down vote for one item.
* Each User has a Reputation that appears next to their name. A User's Reputation begins at 0, and can be positive or negative. Each upvote a user's Question or Answer receives increases their Reputation by 5; each downvote decreases it by 5.
* Users can mark one (and only one) Answer for each of their own Questions as "Correct." This Answer should appear at the top of the Answers list for each Question on their detail page with some indicator.
* Users may be set as Administrators. Administrators have the option to visit an Administrative page which allows them to view all of the Questions on the site, and delete individual Questions.

Bonus Question:
* Add a Seed Method to this application that seeds at least three Users.
