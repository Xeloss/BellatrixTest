# Belatrix Test - Code Review

## To the one reviewing this code reviewing

I included some unit tests that check some data from a database. For easing the process of setting up this kind of tests, i included 
some database files in the test project. These files are automatically deployed when runing the tests, and loaded as a database instance 
with LocalDB. I'm assuming that the one that will check and run these test will have LocalDB installed in his computer. If not, or if he
has some troubles with it, i also included a database project from which one could restore the database required for these tests (consider that 
if using this aproach, the connection string should be changed in the app.config of the test project).
