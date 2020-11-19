using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Utility
{
    public class Message
    {
        public static string invalidLogin = "Invalid login attempt";


        public static string userAdded = "User added successfully";
        public static string userAddedError = "User added un-successfully";
        public static string userUpdated = "User updated successfully";
        public static string userUpdatedError = "User updated un-successfully";
        public static string userDeleted = "User deleted successfully";
        public static string userDeletedError = "User deleted un-successfully";

        public static string categoryAdded = "Category added successfully";
        public static string categoryAddedError = "Category added un-successfully";
        public static string categoryUpdated = "Category updated successfully";
        public static string categoryUpdateError = "Category updated un-successfully";
        public static string categoryDeleted = "Category deleted successfully";
        public static string categoryDeletedError = "Category deleted un-successfully";

        public static string itemTagAdded = "Item tag added successfully";
        public static string itemTagAddedError = "Item tag added un-successfully";
        public static string itemTagUpdated = "Item tag updated successfully";
        public static string itemTagUpdatedError = "Item tag updated un-successfully";
        public static string itemTagDeleted = "Item tag deleted successfully";
        public static string itemTagDeletedError = "Item tag deleted un-successfully";

        public static string menuItemAdded = "Menu item added successfully";
        public static string menuItemAddedError = "Menu item added un-successfully";
        public static string menuItemUpdated = "Menu item updated successfully";
        public static string menuItemUpdatedError = "Menu item updated un-successfully";
        public static string menuItemDeleted = "Menu item deleted successfully";
        public static string menuItemDeletedError = "Menu item deleted un-successfully";
        public static string menuItemNotFound = "Menu item not found";

        public static string menuAdded = "Menu added successfully";
        public static string menuAddedError = "Menu added un-successfully";
        public static string menuUpdated = "Menu updated successfully";
        public static string menuUpdatedError = "Menu updated un-successfully";
        public static string menuDeleted = "Menu deleted successfully";
        public static string menuDeletedError = "Menu deleted un-successfully";

        public static string menuScheduleAdded = "Menu schedule added successfully";
        public static string menuScheduleAddedError = "Menu schedule added un-successfully";
        public static string menuScheduleUpdated = "Menu schedule updated successfully";
        public static string menuScheduleUpdatedError = "Menu schedule updated un-successfully";
        public static string menuScheduleDeleted = "Menu schedule deleted successfully";
        public static string menuScheduleDeletedError = "Menu schedule deleted un-successfully";
        public static string menuScheduleOnSameDateError = "There is already menu schedule on selected date.";

        public static string menuScheduleNotFound = "Menu schedule not found";
        public static string menuScheduleCommingSoon = "Menu schedule is comming soon";
        public static string menuScheduleDateAway = "Menu schedule date is over";

        public static string surveyNotFound = "No any survey found";
        public static string badRequest = "Bad request";
        public static string surveySave = "Survey save successfully";
        public static string surveySaveError = "Survey save un-successfully";

        public static string currencyAdded = "Currency added successfully";
        public static string currencyAddedError = "Currency added un-successfully";
        public static string currencyUpdated = "Currency updated successfully";
        public static string currencyUpdateError = "Currency updated un-successfully";
        public static string currencyDeleted = "Currency deleted successfully";
        public static string currencyDeletedError = "Currency deleted un-successfully";

        public static string conceptAdded = "Concept added successfully";
        public static string conceptAddedError = "Concept added un-successfully";
        public static string conceptUpdated = "Concept updated successfully";
        public static string conceptUpdateError = "Concept updated un-successfully";
        public static string conceptDeleted = "Concept deleted successfully";
        public static string conceptDeletedError = "Concept deleted un-successfully";

        public static string conceptThemeAdded = "Concept theme added successfully";
        public static string conceptThemeAddedError = "Concept theme added un-successfully";
        public static string conceptThemeUpdated = "Concept theme updated successfully";
        public static string conceptThemeUpdateError = "Concept theme updated un-successfully";
        public static string conceptThemeDeleted = "Concept theme deleted successfully";
        public static string conceptThemeDeletedError = "Concept theme deleted un-successfully";

        public static string categorySequence = "Category sequence updated successfully";
        public static string categorySequenceError = "Category sequence updated un-successfully";

        public static string mneuItemSequence = "Menu item sequence updated successfully";
        public static string mneuItemSequenceError = "Menu item sequence updated un-successfully";

        public static string storeAdded = "Store added successfully";
        public static string storeAddedError = "Store added un-successfully";
        public static string storeUpdated = "Store updated successfully";
        public static string storeUpdateError = "Store updated un-successfully";
        public static string storeDeleted = "Store deleted successfully";
        public static string storeDeletedError = "Store deleted un-successfully";
        public static string referenceError = "Unable to delete due to historical data";

        public static string clientAdded = "Client added successfully";
        public static string clientAddedError = "Client added un-successfully";
        public static string clientUpdated = "Client updated successfully";
        public static string clientUpdateError = "Client updated un-successfully";
        public static string clientDeleted = "Client deleted successfully";
        public static string clientDeletedError = "Client deleted un-successfully";

        public static string regionAdded = "Region added successfully";
        public static string regionAddedError = "Region added un-successfully";
        public static string regionUpdated = "Region updated successfully";
        public static string regionUpdateError = "Region updated un-successfully";
        public static string regionDeleted = "Region deleted successfully";
        public static string regionDeletedError = "Region deleted un-successfully";

        public static string countryAdded = "Country added successfully";
        public static string countryAddedError = "Country added un-successfully";
        public static string countryUpdated = "Country updated successfully";
        public static string countryUpdateError = "Country updated un-successfully";
        public static string countryDeleted = "Country deleted successfully";
        public static string countryDeletedError = "Country deleted un-successfully";

        public static string voucherSetupAdded = "Voucher setup added successfully";
        public static string voucherSetupAddedError = "Voucher setup added un-successfully";
        public static string voucherSetupUpdated = "Voucher setup updated successfully";
        public static string voucherSetupUpdatedError = "Voucher setup updated un-successfully";
        public static string voucherSetupDeleted = "Voucher setup deleted successfully";
        public static string voucherSetupDeletedError = "Voucher setup deleted un-successfully";

        public static string voucherIssuanceAdded = "Voucher issuance added successfully";
        public static string voucherIssuanceAddedError = "Voucher issuance added un-successfully";
        public static string voucherIssuanceUpdated = "Voucher issuance updated successfully";
        public static string voucherIssuanceUpdatedError = "Voucher issuance updated un-successfully";
        public static string voucherIssuanceDeleted = "Voucher issuance deleted successfully";
        public static string voucherIssuanceDeletedError = "Voucher issuance deleted un-successfully";

        public static string voucherCategoryAdded = "Voucher category added successfully";
        public static string voucherCategoryAddedError = "Voucher category added un-successfully";
        public static string voucherCategoryUpdated = "Voucher category updated successfully";
        public static string voucherCategoryUpdatedError = "Voucher category updated un-successfully";
        public static string voucherCategoryDeleted = "Voucher category deleted successfully";
        public static string voucherCategoryDeletedError = "Voucher category deleted un-successfully";

        public static string voucherSubCategoryAdded = "Voucher subcategory added successfully";
        public static string voucherSubCategoryAddedError = "Voucher subcategory added un-successfully";
        public static string voucherSubCategoryUpdated = "Voucher subcategory updated successfully";
        public static string voucherSubCategoryUpdatedError = "Voucher subcategory updated un-successfully";
        public static string voucherSubCategoryDeleted = "Voucher subcategory deleted successfully";
        public static string voucherSubCategoryDeletedError = "Voucher subcategory deleted un-successfully";
    }
}
