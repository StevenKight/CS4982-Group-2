CREATE PROCEDURE VerifyUser
    @p_username VARCHAR(100),
    @p_password VARCHAR(100),
    @p_result INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @v_userCount INT;

    SELECT @v_userCount = COUNT(*)
    FROM [dbo].[User]
    WHERE [username] = @p_username AND [password] = @p_password;

    IF @v_userCount = 0
        SET @p_result = 1;
    ELSE
        SET @p_result = 0;
END;
