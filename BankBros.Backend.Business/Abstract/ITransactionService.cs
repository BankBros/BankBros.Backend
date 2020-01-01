using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.Entity.Dtos;

namespace BankBros.Backend.Business.Abstract
{
    public interface ITransactionService
    {
        IResult Transfer(int userId, TransferDto transfer);
        IResult Virement(int userId, VirementDto virementDto);
        IResult Draw(int userId, DrawDto drawDto);
        IResult Deposit(int userId, DepositDto depositDto);
        IResult PayBill(int customerId, PayBillDto payBillDto);

    }
}
