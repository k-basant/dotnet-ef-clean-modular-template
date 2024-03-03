**DB Manager**

*Possibility of 1 or more DbContexts*
- You can add multiple AppDbContexts for your application as per the requirement.
	
	Add-Migration: 
	
	For Context 1: `Add-Migration Init -OutputDir Module1/Migrations -Context AppDbContextExt`
	
	For Context 2: `Add-Migration Init -OutputDir Module2/Migrations -Context AppDbContext2Ext`

	Update-Database:
	
	For Context 1: ` Update-Database -Context AppDbContextExt`
	
	For Context 2: `Update-Database -Context AppDbContext2Ext`

- Important Facts
	- Your DbContexts in the DBManager need not to be exact same replica of your Infrastructure layer DbContexts.
		For e.g. - In your Infrastructure layer you might have 1 DbContext representing the whole DB whereas DbManager may have 2 or more per module basis to better maintain migrations or DB changes.
	- It's suggested to use `Table["TableName", "SchemaName"]` attribute explicitly on all the domain model classes if your application tends to have multiple schemas.
