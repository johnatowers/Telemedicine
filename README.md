# Telemedicine
-----
# if this branch isn't working
Check the patient-appointments or doctor-appointments component to see if there are calendar components that you need to install. google them to find the npm install statements to run under Telemedicine-SPA/

It could also be a database problem due to adding the Appointments table. if you think this is the case, first try: 
1. under Telemedicine.API/, run: dotnet ef database drop
2. dotnet watch run

if the errror still occurs, try:
1. under Telemedicine.API/, run: dotnet ef database drop
2. delete the entire migrations folder under Telemedicine.API/
3. under Telemedicine.API/, run: dotnet ef migrations add AppointmentAddMigration
4. under Telemedicine.API/, run: dotnet watch run

-----
# User Role Information
Our user class contains a new field, UserRole of type UserRole. The UserRole class itself contains a field for a Role of type Role and a User of type User. The Role class contains a field called UserRoles which is a collection of type UserRole. This isn't super important information but shows the structure. The database relationship (one-to-one, one-to-many, whatever) is defined in the datacontext. if you should need to know that stuff.

When a user registrates from our main registration screen, they are automatically assigned the role of "Patient" (AuthController.cs Register method does this).
We now have one admin account, username "admin" and password "admin" (which was automatically added when I "seeded" data after we drop the DB and re-add it.)
If you want to access the role for a particular user:
string Role = user.UserRole.Role.ToString()
this will equal either, "Admin", "Patient, or "Doctor" (doctor hasn't been implemented anywhere yet though)

Using this to customize the view for the role of the user is pretty simple, you can watch video 222 from section 21 (maybe 220, 223 too for angular stuff) 
The only part of this I have implemented so far is if you login as the admin you will have an "admin" tab at the top, but any patient user can not see that. We can remove the patient and doctor user tabs from the admin view the same way, and add a page to the admin view for registering a doctor, etc.

-----
# Database information

You'll notice that the database looks different after adding the roles. Every table as "AspNet" before it and there are a lot of extra ones. the important ones are "AspNetUserRoles" which shows the userId and the Id of the their role. and "AspNetUsers" which is the usual user table. 

-----
# TO DO:
- Video Chat (Vishal working on)
- Connect edit profile to front end chart
- Highlighting selected buttons
- Doctor profile page (Vishal creating, Macy will route)
- Week view of appts on my chart
- Member card component (make name bigger, configure buttons)
- Once in relationship with doctor/patient, make plus button disappear 
- Add more of a variety of color rather than all light blue





