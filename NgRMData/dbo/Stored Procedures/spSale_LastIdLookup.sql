CREATE PROCEDURE [dbo].[spSale_LastIdLookup]
	
AS
begin
set nocount on;
	SELECT TOP 1 Id as Id FROM dbo.Sale ORDER BY Id DESC ;
	end


