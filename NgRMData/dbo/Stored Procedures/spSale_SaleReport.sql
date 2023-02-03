CREATE PROCEDURE [dbo].[spSale_SaleReport]
	--Basic sale report
AS
begin
set nocount on;
	Select  [s].[SaleDate], [s].[Subtotal], [s].[Tax], [s].[Total], u.FirstName, u.LastName, u.EmailAdress FROM dbo.Sale s
	inner join dbo.[User] u on s.CashierId = u.AuthUserId
end
