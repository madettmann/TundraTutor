# Tundra
<strong>Tutoring System</strong><br>
This program allows tutees and tutors to schedule appointments efficiently.

In the event that the database is generating errors (anything with SQL in the exception name, as well as errors about "startenddate" when adding busies), you will need to go to the TutoringTables.edmx file within the TutoringDB, then right click anywhere not on a table and select "Update Model from Database" in the context menu. From there, navigate to the Refresh tab, click on "Tables" and click "Finish". This will reset the database, meaning that in order to schedule any busy times, the admin must set the StartDate and EndDate of the period, and then use the generate functions to fill the database properly again. The reason this happens is because the Debug folder is ignored by the standard gitignore and would have no bearing on the functioning of a deployed application.

If nothing else is working, you the zipped project in the top directory, it should work regarless of anything else. 

Written using Visual Studio 2017 RC
