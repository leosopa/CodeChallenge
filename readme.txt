There is 2 projects:

- ChatService - Visual Studio Solution
- mychat.frontend - A React Application for chat


Steps:

1 - Open ChatService solution.
2 - Execute migrations (SQLExpress Database):
	2.1 - Add-Migrations "Initial-Create"
	2.2 - UpdateDatabase
3 - npm start in mychat.frontend folder
4 - Open http://localhost:3000/ for chat
	4.1 - There is one room in migrations
		4.1.1 - Main Room
	4.2 - There is 2 users in migrations
		4.2.1 - user1 - password:123
		4.2.2 - user2 - password:123
