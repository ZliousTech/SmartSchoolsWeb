/*
Default code generation for IDataAcccessFactory Class ,this code will be overwrited every time the code been generated 
in case to add any custom code please add it in IDataAccessFactoryCustom class 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Repositories;
using DataAccess.IRepositories;
using Objects;

namespace DataAccess.Base
{
public partial interface IDataAccessFactory
{

    IAbsenceReasonsRepository GetAbsenceReasonsRepository { get; }
        
    IAcademicCalendarsRepository GetAcademicCalendarsRepository { get; }
        
    IAcademicCalendar1sRepository GetAcademicCalendar1sRepository { get; }
        
    IAccountOwnerTypesRepository GetAccountOwnerTypesRepository { get; }
        
    IAccountsDetailsRepository GetAccountsDetailsRepository { get; }
        
    IAccountsMastersRepository GetAccountsMastersRepository { get; }
        
    IAccountTypesRepository GetAccountTypesRepository { get; }
        
    IActiveInstalltionsRepository GetActiveInstalltionsRepository { get; }
        
    IActiveProductKeysRepository GetActiveProductKeysRepository { get; }
        
    IAgeClassificationsRepository GetAgeClassificationsRepository { get; }
        
    IAppraisalsRepository GetAppraisalsRepository { get; }
        
    IAppraisalMetricsGroupsRepository GetAppraisalMetricsGroupsRepository { get; }
        
    IAppraisalTypesRepository GetAppraisalTypesRepository { get; }
        
    IAppraisalValuesRepository GetAppraisalValuesRepository { get; }
        
    IAreasRepository GetAreasRepository { get; }
        
    IAspNetRolesRepository GetAspNetRolesRepository { get; }
        
    IAttendancesRepository GetAttendancesRepository { get; }
        
    IAttendanceTypesRepository GetAttendanceTypesRepository { get; }
        
    IAutomaticTimetablesRepository GetAutomaticTimetablesRepository { get; }
        
    IBanksRepository GetBanksRepository { get; }
        
    IBankBranchsRepository GetBankBranchsRepository { get; }
        
    IBloodTypesRepository GetBloodTypesRepository { get; }
        
    IBooksRepository GetBooksRepository { get; }
        
    IBookChaptersRepository GetBookChaptersRepository { get; }
        
    IBookUnitsRepository GetBookUnitsRepository { get; }
        
    IBusAttendanceSchedulesRepository GetBusAttendanceSchedulesRepository { get; }
        
    IBusFuelsRepository GetBusFuelsRepository { get; }
        
    IBusInfosRepository GetBusInfosRepository { get; }
        
    IBusMaintenancesRepository GetBusMaintenancesRepository { get; }
        
    IBusShiftRotationSchedulesRepository GetBusShiftRotationSchedulesRepository { get; }
        
    IBusShiftRotationScheduleforAutomaticBusSchedulesRepository GetBusShiftRotationScheduleforAutomaticBusSchedulesRepository { get; }
        
    IBusToursRepository GetBusToursRepository { get; }
        
    IBusTrackNavigationsRepository GetBusTrackNavigationsRepository { get; }
        
    IBusTripsRepository GetBusTripsRepository { get; }
        
    IClasssRepository GetClasssRepository { get; }
        
    IClassroomsRepository GetClassroomsRepository { get; }
        
    ICountrysRepository GetCountrysRepository { get; }
        
    ICreditCardTypesRepository GetCreditCardTypesRepository { get; }
        
    ICurrencysRepository GetCurrencysRepository { get; }
        
    ICurriculumsRepository GetCurriculumsRepository { get; }
        
    ICurriculumDepartmentsRepository GetCurriculumDepartmentsRepository { get; }
        
    ICustomPropertyTypesRepository GetCustomPropertyTypesRepository { get; }
        
    IDepartmentsRepository GetDepartmentsRepository { get; }
        
    IDesignationsRepository GetDesignationsRepository { get; }
        
    IDesignationTypesRepository GetDesignationTypesRepository { get; }
        
    IDeviceRegistrarsRepository GetDeviceRegistrarsRepository { get; }
        
    IDisciplineActionsRepository GetDisciplineActionsRepository { get; }
        
    IDiscountsRepository GetDiscountsRepository { get; }
        
    IDiscountStudentsRepository GetDiscountStudentsRepository { get; }
        
    IDiscountTypesRepository GetDiscountTypesRepository { get; }
        
    IDocumentsRepository GetDocumentsRepository { get; }
        
    IDocumentCustomPropertysRepository GetDocumentCustomPropertysRepository { get; }
        
    IDocumentTypesRepository GetDocumentTypesRepository { get; }
        
    IDocumrntCustomizedPropertysRepository GetDocumrntCustomizedPropertysRepository { get; }
        
    IEducationalYearsRepository GetEducationalYearsRepository { get; }
        
    IExternalGuardiansRepository GetExternalGuardiansRepository { get; }
        
    IExternalGuardStudentsRequestsRepository GetExternalGuardStudentsRequestsRepository { get; }
        
    IExternalOtherStudentDetailsRepository GetExternalOtherStudentDetailsRepository { get; }
        
    IExternalStudentsRepository GetExternalStudentsRepository { get; }
        
    IExternalStudentAdresssRepository GetExternalStudentAdresssRepository { get; }
        
    IExternalStudentGuardDetailsRepository GetExternalStudentGuardDetailsRepository { get; }
        
    IExternalStudentSchoolDetailsRepository GetExternalStudentSchoolDetailsRepository { get; }
        
    IFamilysRepository GetFamilysRepository { get; }
        
    IFeesRepository GetFeesRepository { get; }
        
    IFeeTypesRepository GetFeeTypesRepository { get; }
        
    IFunctionsRepository GetFunctionsRepository { get; }
        
    IGroupsRepository GetGroupsRepository { get; }
        
    IGuardiansRepository GetGuardiansRepository { get; }
        
    IGuardianTypesRepository GetGuardianTypesRepository { get; }
        
    IHeadquarterssRepository GetHeadquarterssRepository { get; }
        
    IImportanceTypesRepository GetImportanceTypesRepository { get; }
        
    IKPIsRepository GetKPIsRepository { get; }
        
    ILanguagesRepository GetLanguagesRepository { get; }
        
    ILiveswithTypesRepository GetLiveswithTypesRepository { get; }
        
    ILogsRepository GetLogsRepository { get; }
        
    ILookupValuesRepository GetLookupValuesRepository { get; }
        
    IMailTemplatesRepository GetMailTemplatesRepository { get; }
        
    IMaintenanceTypesRepository GetMaintenanceTypesRepository { get; }
        
    IManualTimetablesRepository GetManualTimetablesRepository { get; }
        
    IMappedFieldsRepository GetMappedFieldsRepository { get; }
        
    IMaritalStatussRepository GetMaritalStatussRepository { get; }
        
    IMSTeams_AccountssRepository GetMSTeams_AccountssRepository { get; }
        
    IMSTeams_SessionAttendessRepository GetMSTeams_SessionAttendessRepository { get; }
        
    IMSTeams_SessionssRepository GetMSTeams_SessionssRepository { get; }
        
    INotifiactionStatussRepository GetNotifiactionStatussRepository { get; }
        
    INotificationsRepository GetNotificationsRepository { get; }
        
    INotificationsWebsRepository GetNotificationsWebsRepository { get; }
        
    IOccasionTypesRepository GetOccasionTypesRepository { get; }
        
    IOtherStudentDetailsRepository GetOtherStudentDetailsRepository { get; }
        
    IPaymentMethodsRepository GetPaymentMethodsRepository { get; }
        
    IPrivilagesRepository GetPrivilagesRepository { get; }
        
    IQualificationsRepository GetQualificationsRepository { get; }
        
    IQuestionBanksRepository GetQuestionBanksRepository { get; }
        
    IQuestionLevelsRepository GetQuestionLevelsRepository { get; }
        
    IQuestionTypesRepository GetQuestionTypesRepository { get; }
        
    IRecurrenceTypesRepository GetRecurrenceTypesRepository { get; }
        
    IReligionsRepository GetReligionsRepository { get; }
        
    IScheduledBusTripsRepository GetScheduledBusTripsRepository { get; }
        
    IScheduleZonesRepository GetScheduleZonesRepository { get; }
        
    ISchoolBranchsRepository GetSchoolBranchsRepository { get; }
        
    ISchoolClasssRepository GetSchoolClasssRepository { get; }
        
    ISchoolSettingsRepository GetSchoolSettingsRepository { get; }
        
    ISectionsRepository GetSectionsRepository { get; }
        
    ISessionsRepository GetSessionsRepository { get; }
        
    ISessionDistributionsRepository GetSessionDistributionsRepository { get; }
        
    ISpecializationsRepository GetSpecializationsRepository { get; }
        
    ISpecialResidenceConditionTypesRepository GetSpecialResidenceConditionTypesRepository { get; }
        
    IStaffsRepository GetStaffsRepository { get; }
        
    IStaffBankDetailsRepository GetStaffBankDetailsRepository { get; }
        
    IStaffContactDetailsRepository GetStaffContactDetailsRepository { get; }
        
    IStaffJobDetailsRepository GetStaffJobDetailsRepository { get; }
        
    IStaffLeafsRepository GetStaffLeafsRepository { get; }
        
    IStaffSalaryDetailsRepository GetStaffSalaryDetailsRepository { get; }
        
    IStudentsRepository GetStudentsRepository { get; }
        
    IStudent_LoginsRepository GetStudent_LoginsRepository { get; }
        
    IStudentAdresssRepository GetStudentAdresssRepository { get; }
        
    IStudentBehaviorsRepository GetStudentBehaviorsRepository { get; }
        
    IStudentDiseassRepository GetStudentDiseassRepository { get; }
        
    IStudentFeesRepository GetStudentFeesRepository { get; }
        
    IStudentGuardDetailsRepository GetStudentGuardDetailsRepository { get; }
        
    IStudentHealthsRepository GetStudentHealthsRepository { get; }
        
    IStudentPaymentsRepository GetStudentPaymentsRepository { get; }
        
    IStudentPhysicalStatusRepository GetStudentPhysicalStatusRepository { get; }
        
    IStudentResultsRepository GetStudentResultsRepository { get; }
        
    IStudentSchoolDetailsRepository GetStudentSchoolDetailsRepository { get; }
        
    IStudentStatusRepository GetStudentStatusRepository { get; }
        
    ISubjectsRepository GetSubjectsRepository { get; }
        
    ISystemFieldsRepository GetSystemFieldsRepository { get; }
        
    ISystemObjectsRepository GetSystemObjectsRepository { get; }
        
    ISystemSettingsRepository GetSystemSettingsRepository { get; }
        
    ISystemSettingsperSchoolsRepository GetSystemSettingsperSchoolsRepository { get; }
        
    ITeachersRepository GetTeachersRepository { get; }
        
    ITeacherExperiencesRepository GetTeacherExperiencesRepository { get; }
        
    ITeacherSubjectsRepository GetTeacherSubjectsRepository { get; }
        
    ITemplateSettingsRepository GetTemplateSettingsRepository { get; }
        
    ITimetablesRepository GetTimetablesRepository { get; }
        
    ITimetableConditionsRepository GetTimetableConditionsRepository { get; }
        
    ITimetableItemsRepository GetTimetableItemsRepository { get; }
        
    ITransportCategorysRepository GetTransportCategorysRepository { get; }
        
    ITransportCategoryTypesRepository GetTransportCategoryTypesRepository { get; }
        
    ITransportDirectionsRepository GetTransportDirectionsRepository { get; }
        
    ITransportTypesRepository GetTransportTypesRepository { get; }
        
    IUsersRepository GetUsersRepository { get; }
        
    IUserTypesRepository GetUserTypesRepository { get; }
        
    IVirtualMeetingsRepository GetVirtualMeetingsRepository { get; }
        
    IWebUsersRepository GetWebUsersRepository { get; }
        
    IWifesRepository GetWifesRepository { get; }
        
}
}


