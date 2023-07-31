--Run this using sa login

Declare @DatabaseName varchar(MAX) = '< Database Name >' --replace < Database Name >
Declare @DatabaseuserID varchar(MAX) = '< Database UserID >' --replace < Database UserID >

EXEC('
ALTER DATABASE ['+@DatabaseName+'] SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE
ALTER DATABASE ['+@DatabaseName+'] SET TRUSTWORTHY ON; 
GRANT ALTER TO ['+@DatabaseuserID+']

GRANT CREATE PROCEDURE TO ['+@DatabaseuserID+'];
GRANT CREATE SERVICE TO ['+@DatabaseuserID+'];
GRANT CREATE QUEUE TO ['+@DatabaseuserID+'];
GRANT REFERENCES ON CONTRACT::[DEFAULT] TO ['+@DatabaseuserID+'];
GRANT SUBSCRIBE QUERY NOTIFICATIONS TO ['+@DatabaseuserID+'];
GRANT RECEIVE ON QueryNotificationErrorsQueue TO ['+@DatabaseuserID+']
')

if not exists (SELECT is_broker_enabled,is_trustworthy_on FROM sys.databases 
WHERE name = ''+@DatabaseName+'' AND (is_broker_enabled = 0 OR is_trustworthy_on = 0)) BEGIN
	select 'SUCCESS'
END ELSE BEGIN
	select 'FAILED'
END GO
