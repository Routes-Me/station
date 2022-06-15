using System;
using RoutesStation.ModelReturn;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Wallet
{
	public interface IWalletRep
	{
		Task<IEnumerable<ApplicationWalletCharging>> ListChrgingWallet();
		Task<IEnumerable<ApplicationWalletTrip>> ListPaymentWallet();
		Task<ApplicationGeneralListWithCountSum<ApplicationWalletTrip>> ListPaymentWalletByUser(ApplicationPagination pagination);
		Task<ApplicationGeneralListWithCountSum<ApplicationWalletCharging>> ListChrgingWalletByUser(ApplicationPagination pagination);
		Task<IEnumerable<ApplicationWallet>> ListWallet();
		Task<double> SumChargeWallet();
		Task<double> SumPaymentWallet();
		Task<ApplicationWallet> Wallet(ApplicationUser user);
		Task<StatuseModel> ChargeWallet(ApplicationCahrgWalletView cahrgWalletView,ApplicationUser user);
		Task<StatuseModel> ChargeWalletByPromoter(ApplicationCahrgWalletView cahrgWalletView, ApplicationUser user);
		Task<StatuseModel> ChargeWalletByInspector(ApplicationCahrgWalletView cahrgWalletView, ApplicationUser user);
		Task<StatuseModel> PaymentWallet(ApplicationPaymentWalletView paymentWalletView,ApplicationUser user);
		Task<StatuseModel> PaymentBySecondID(ApplicationPaymentBySecondID paymentBySecondID, ApplicationUser user);
		Task<StatuseModel> PaymentWalletByInspector(ApplicationPaymentWalletView paymentWalletView, ApplicationUser user);
		Task<ApplicationWalletTrip> GetPayment(Guid PaymentID);
		Task<int> CountCharge();
		Task<int> CountPayment();
		Task<IEnumerable<ApplicationWalletTrip>> ListPaymentWalletByBusCompany(Guid CompanyID);
		Task<ApplicationBusDriverMap> GetBusAndDriverActive(Guid BusID,string DriverID);
		Task<StatuseModel> CheckPayment(ApplicationCheckPaymentView checkPaymentView);

		

	}
	
}

