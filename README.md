# DOT-NET-File-Upload-Website
.NET File Upload website using MySQL for file and user storage

Usage
Clone the repository and open in Visual studio

In this project, users can create accounts to upload files and have anonymous users to be able to download them. There are limitations and security risks which will be implemented in the future including, but not limited to: user authentication and integration as currently, this is only handled through sessions; other risks are simply built on top of this, such as potentially being able to edit another user's posts due to the lack of authorization limitations. While an email is required to sign up, it currently has no use. User sign-up and sign-in validation has to be implemented. The functionality of uploading and downloading have been implemented, however, and allows authenticated and anonymous users to search and download files on the website.

For this project, MySQL was used to store both file information and user information. No front-end technology was used, but an MVC architecture was used.
