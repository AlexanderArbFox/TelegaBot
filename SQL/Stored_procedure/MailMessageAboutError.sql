-- =============================================
-- Author:		<Alex>
-- Create date: <16.04.22>
-- Description:	<this procedure made for message an mail to admin>
-- =============================================
CREATE PROCEDURE MailMessageAboutError
@ErrorMessage nvarchar(max)
as 
begin
set nocount on;
declare @body_text nvarchar(max) = char(25) + '��������� ������' + @ErrorMessage
exec msdb.dbo.sp_send_dbmail
@profile_name = 'TelegaBot'
,@recipients = N'"Ivanov Ivan Ivanovich" www.arbuzfox11@mail.ru'
,@body = @body_text
,@subject = '������ ������ ��������� ����'
,@importance = 'high'

end