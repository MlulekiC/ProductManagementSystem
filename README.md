Project Overview

ProductMS is a web based application that enables users to manage their inventory, it prioritises security with features like email/account confirmation and password encryption ensuring sensitive data is always hidden.

NOTE: /*The end goal was for this project to be hosted locally via IIS, but I ran out of time before figuring out the required IIS configuration. This directly affects things like the email confirmation redirect link functionality.*/

Setup
1.	Please ensure that all the packages referenced by both projects in the Solution are all downloaded.
2.	If having issues sending the email, kindly override UsersController> line 70 with: if (true)
3.	You need to update the Connected services details to match your local SQL server instance and database:
<img width="795" height="194" alt="image" src="https://github.com/user-attachments/assets/6a3aa4d6-5d67-407c-8b89-19557a021389" />

Landing Page

This is the first page the user sees after successful compilation/building:
<img width="591" height="473" alt="image" src="https://github.com/user-attachments/assets/7d9f8090-c152-4e82-882b-f89645aa1813" />


If it is your first time running the app, you can opt to create an account by selecting ‘Don’t have an Account’.
<img width="578" height="478" alt="image" src="https://github.com/user-attachments/assets/91d51d58-d205-40cc-be27-5bd32d667a32" />


Upon successful capture of the required details and clicking create, an account confirmation email will be sent to the captured email address. 
The app then redirects the user back to the login page, where they can login using their newly created details


Product Listing

<img width="939" height="344" alt="image" src="https://github.com/user-attachments/assets/ad656f66-8a0a-4404-8932-7636c3ad9c56" />
This page only shows products created by the logged in user, so if you are logging in for the first time you will not see any records on the screen and you will have to create a new product by selecting ‘Create New’.
This page also enables users to navigate to the Category listing screen, since you can not add a new product without specifying its Category.


Add new Product

<img width="497" height="469" alt="image" src="https://github.com/user-attachments/assets/ec06ffa4-8467-4b31-95f9-560a95269a64" />

After capturing all product details and clicking create, the user is returned to the Product listing screen.

ERD

<img width="1004" height="561" alt="image" src="https://github.com/user-attachments/assets/aa839149-911d-4d89-9cdc-bbe8cb714a86" />
