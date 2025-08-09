# OurMarketBackend

OurMarket is an ASP.NET Core MVC web application that functions 
as a marketplace where users can post listings and message each other.

# Features
- User registration, login, and authentication
- Create, view, and manage listings
- Messaging system for users to communicate privately
- Inbox and conversation views for message management

# Technologies Used
- ASP.NET Core MVC
- Entity Framework Core
- Identity for user management
- SQLite

# Setup Instructions
1. Clone the repository:   
   git clone https://github.com/sobstori/OURMARKET.git

   cd OurMarketBackend
   
3. Configure your database connection string in appsettings.json.

4. Run the migrations and update the database:
    dotnet ef database update

5. Run the application:
    dotnet run

6. Open your browser from the URL shown in your terminal.


# Testing the Messaging System
  To verify the messaging functionality works properly, follow these steps:

    1. Run the application using dotnet run.
    2. Open two different browsers or use an incognito window so you can log in as two different users simultaneously.
    3. Create two separate user accounts by registering on the site with different email addresses.
    4. Log in as User A in one browser and browse the listings. On a listing posted by User B, click the Message Seller button.
    5. Send a message to User B using the messaging form.
    6. Switch to User B’s browser window and navigate to the Inbox or Messages section.
    7. You should see a link to the message, click, and you'll see the conversation with User A and the message you just sent.
    8. Reply from User B to User A and verify that the messages appear correctly in both users’ conversations. (refresh the page)

