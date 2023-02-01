CREATE PROCEDURE [dbo].[SpSale_Insert]
	@Id int output,
	@CashierId nvarchar(128),
	@SaleDate datetime2,
	@SubTotal money,
	@Tax money,
	@Total money
AS
begin 
set nocount on;
	INSERT INTO dbo.Sale(CashierId,SaleDate,Subtotal,Tax,Total) 
	values(@CashierId,@SaleDate,@Subtotal,@Tax,@Total);
	
	Select @Id = @@IDENTITY;

end
