namespace CSharpEfAndAspNet;

public sealed class Account
{
    public decimal Balance { get; set; }
}

public sealed class PaymentProccessor
{
    public void Payout(Account? account, decimal amount)
    {
        account?.Balance -= amount;
    }
}
