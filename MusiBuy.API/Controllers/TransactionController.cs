using Microsoft.AspNetCore.Mvc;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
using MusiBuy.Common.ResponseModels;
using static MusiBuy.Common.Models.API.PostAndEventViewModel;
using static MusiBuy.Common.Models.API.TransactionViewModel;

namespace MusiBuy.API.Controllers
{
    [Route("api/Transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        #region Member Declaration
        private readonly IBank _bankRepo;
        private readonly ITransaction _transaction;
        public TransactionController(IBank bankRepo, ITransaction transaction)
        {
            _bankRepo = bankRepo;
            _transaction = transaction;
        }
        #endregion

        #region Save Bank by User id
        [HttpPost("SaveBank")]
        [EndpointSummary("SaveBank")]
        [EndpointDescription("Save Bank By UserId")]
        [EndpointName("SaveBank")]
        public IActionResult SaveBank(ApiBankMaster bankMaster)
        {
            if (bankMaster.UserId == 0)
                return Ok(new ApiResponse(false, "Invalid User", null));

            if (string.IsNullOrEmpty(bankMaster.BankName))
                return Ok(new ApiResponse(false, "Enter Bank Name", null));

            if (string.IsNullOrEmpty(bankMaster.AccountNumber))
                return Ok(new ApiResponse(false, "Enter Account No.", null));

            if (string.IsNullOrEmpty(bankMaster.IFSCCode))
                return Ok(new ApiResponse(false, "Enter IFSC/SWIFT Code", null));

            BankMasterViewModel bankMasterViewModel = new BankMasterViewModel();
            bankMasterViewModel.Id = bankMaster.Id;
            bankMasterViewModel.UserId = bankMaster.UserId;
            bankMasterViewModel.BankName = bankMaster.BankName;
            bankMasterViewModel.AccountNumber = bankMaster.AccountNumber;
            bankMasterViewModel.IFSCCode = bankMaster.IFSCCode;
            bankMasterViewModel.AccountHolder = bankMaster.AccountHolder;
            bankMasterViewModel.IsActive = true;
            if (bankMaster.Id == 0)
            {
                bankMasterViewModel.IsSelected = true;
                bankMasterViewModel.CreatedBy = bankMaster.UserId;
            }
            else
            {
                bankMasterViewModel.UpdatedBy = bankMaster.UserId;
            }


            bool IsSave = _bankRepo.Save(bankMasterViewModel);
            if (IsSave)
                return Ok(new ApiResponse(true, "Bank Added", null));
            else
                return Ok(new ApiResponse(false, "Error While Save Bank Detail", null));
        }
        #endregion

        #region Get Bank List By UserId
        [HttpPost("GetBankByUserId")]
        [EndpointSummary("GetBankByUserId")]
        [EndpointDescription("Get Bank By UserId")]
        [EndpointName("GetBankByUserId")]
        public IActionResult GetBankByUserId(CommonModel commonModel)
        {
            if (commonModel.Id == 0)
                return Ok(new ApiResponse(false, "Invalid User", null));


            var BankDetail = _bankRepo.GetBankListByUserId(commonModel.Id);
            if (BankDetail.Count == 0)
                return Ok(new ApiResponse(true, "No Record Found.", null));

            var result = BankDetail.Select(r => new
            {
                r.Id,
                r.BankName,
                r.AccountNumber,
                r.IFSCCode,
                r.AccountHolder,
                r.IsSelected,
            });
            return Ok(new ApiResponse(true, "Sucess", result));
        }
        #endregion

        #region Select Bank By UserId
        [HttpPost("SelectBank")]
        [EndpointSummary("SelectBank")]
        [EndpointDescription("Select Bank By UserId And BankId")]
        [EndpointName("SelectBank")]
        public IActionResult SelectBank(GetByUserId getByUser)
        {
            if (getByUser.UserId == 0)
                return Ok(new ApiResponse(false, "Invalid User", null));

            if (getByUser.Id == 0)
                return Ok(new ApiResponse(false, "Invalid Bank", null));


            bool IsSelected = _bankRepo.SelectByBankId(getByUser);
            if (IsSelected)
                return Ok(new ApiResponse(true, "Bank Selected.", null));
            else
                return Ok(new ApiResponse(true, "Error While Select Bank", null));
        }
        #endregion

        #region Delete Bank By UserId
        [HttpPost("DeleteBank")]
        [EndpointSummary("DeleteBank")]
        [EndpointDescription("Delete Bank By UserId And BankId")]
        [EndpointName("DeleteBank")]
        public IActionResult DeleteBank(GetByUserId getByUser)
        {
            if (getByUser.UserId == 0)
                return Ok(new ApiResponse(false, "Invalid User", null));

            if (getByUser.Id == 0)
                return Ok(new ApiResponse(false, "Invalid Bank", null));


            bool IsSelected = _bankRepo.DeletebankByBankId(getByUser);
            if (IsSelected)
                return Ok(new ApiResponse(true, "Bank Deleted.", null));
            else
                return Ok(new ApiResponse(true, "Error While Delete Bank", null));
        }
        #endregion


        #region Get Token Pricing List For Purchase
        [HttpPost("TokenPlanList")]
        [EndpointSummary("TokenPlanList")]
        [EndpointDescription("Token Pricing List For Purchase")]
        [EndpointName("TokenPlanList")]
        public IActionResult TokenPlanList()
        {
            var PlanList = _transaction.GetTokenPlanList();

            if (PlanList.Count == 0)
                return Ok(new ApiResponse(true, "No Record Found.", null));

            var result = PlanList.Select(r => new
            {
                r.Id,
                r.TokenTypeName,
                TokenQuantity = r.TokenQuantity + " Token",
                Price = "$ " + r.Price,
                r.Description,
            });
            return Ok(new ApiResponse(true, "Sucess", result));
        }
        #endregion


    }
}
