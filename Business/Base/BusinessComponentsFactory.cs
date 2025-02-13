/*
Default code generation for BusinessComponentsFactory Class ,this code will be overwrited every time the code been generated 
in case to add any custom code please add it in BusinessComponentsFactoryCustom class 
*/
using Business.Interfaces;
using Business.Managers;
using DataAccess.Base;

namespace Business.Base
{
    public partial class BusinessComponentsFactory : IBusinessComponentsFactory
    {


        public IAbsenceReasonsManager CreateAbsenceReasonsManager()
        {
            return new AbsenceReasonsManager(new DataAccessFactory());
        }

        public IAcademicCalendarsManager CreateAcademicCalendarsManager()
        {
            return new AcademicCalendarsManager(new DataAccessFactory());
        }

        public IAcademicCalendar1sManager CreateAcademicCalendar1sManager()
        {
            return new AcademicCalendar1sManager(new DataAccessFactory());
        }

        public IAccountOwnerTypesManager CreateAccountOwnerTypesManager()
        {
            return new AccountOwnerTypesManager(new DataAccessFactory());
        }

        public IAccountsDetailsManager CreateAccountsDetailsManager()
        {
            return new AccountsDetailsManager(new DataAccessFactory());
        }

        public IAccountsMastersManager CreateAccountsMastersManager()
        {
            return new AccountsMastersManager(new DataAccessFactory());
        }

        public IAccountTypesManager CreateAccountTypesManager()
        {
            return new AccountTypesManager(new DataAccessFactory());
        }

        public IActiveInstalltionsManager CreateActiveInstalltionsManager()
        {
            return new ActiveInstalltionsManager(new DataAccessFactory());
        }

        public IActiveProductKeysManager CreateActiveProductKeysManager()
        {
            return new ActiveProductKeysManager(new DataAccessFactory());
        }

        public IAgeClassificationsManager CreateAgeClassificationsManager()
        {
            return new AgeClassificationsManager(new DataAccessFactory());
        }

        public IAppraisalsManager CreateAppraisalsManager()
        {
            return new AppraisalsManager(new DataAccessFactory());
        }

        public IAppraisalMetricsGroupsManager CreateAppraisalMetricsGroupsManager()
        {
            return new AppraisalMetricsGroupsManager(new DataAccessFactory());
        }

        public IAppraisalTypesManager CreateAppraisalTypesManager()
        {
            return new AppraisalTypesManager(new DataAccessFactory());
        }

        public IAppraisalValuesManager CreateAppraisalValuesManager()
        {
            return new AppraisalValuesManager(new DataAccessFactory());
        }

        public IAreasManager CreateAreasManager()
        {
            return new AreasManager(new DataAccessFactory());
        }

        public IAspNetRolesManager CreateAspNetRolesManager()
        {
            return new AspNetRolesManager(new DataAccessFactory());
        }

        public IAttendancesManager CreateAttendancesManager()
        {
            return new AttendancesManager(new DataAccessFactory());
        }

        public IAttendanceTypesManager CreateAttendanceTypesManager()
        {
            return new AttendanceTypesManager(new DataAccessFactory());
        }

        public IAutomaticTimetablesManager CreateAutomaticTimetablesManager()
        {
            return new AutomaticTimetablesManager(new DataAccessFactory());
        }

        public IBanksManager CreateBanksManager()
        {
            return new BanksManager(new DataAccessFactory());
        }

        public IBankBranchsManager CreateBankBranchsManager()
        {
            return new BankBranchsManager(new DataAccessFactory());
        }

        public IBloodTypesManager CreateBloodTypesManager()
        {
            return new BloodTypesManager(new DataAccessFactory());
        }

        public IBooksManager CreateBooksManager()
        {
            return new BooksManager(new DataAccessFactory());
        }

        public IBookChaptersManager CreateBookChaptersManager()
        {
            return new BookChaptersManager(new DataAccessFactory());
        }

        public IBookUnitsManager CreateBookUnitsManager()
        {
            return new BookUnitsManager(new DataAccessFactory());
        }

        public IBusAttendanceSchedulesManager CreateBusAttendanceSchedulesManager()
        {
            return new BusAttendanceSchedulesManager(new DataAccessFactory());
        }

        public IBusFuelsManager CreateBusFuelsManager()
        {
            return new BusFuelsManager(new DataAccessFactory());
        }

        public IBusInfosManager CreateBusInfosManager()
        {
            return new BusInfosManager(new DataAccessFactory());
        }

        public IBusMaintenancesManager CreateBusMaintenancesManager()
        {
            return new BusMaintenancesManager(new DataAccessFactory());
        }

        public IBusShiftRotationSchedulesManager CreateBusShiftRotationSchedulesManager()
        {
            return new BusShiftRotationSchedulesManager(new DataAccessFactory());
        }

        public IBusShiftRotationScheduleforAutomaticBusSchedulesManager CreateBusShiftRotationScheduleforAutomaticBusSchedulesManager()
        {
            return new BusShiftRotationScheduleforAutomaticBusSchedulesManager(new DataAccessFactory());
        }

        public IBusToursManager CreateBusToursManager()
        {
            return new BusToursManager(new DataAccessFactory());
        }

        public IBusTrackNavigationsManager CreateBusTrackNavigationsManager()
        {
            return new BusTrackNavigationsManager(new DataAccessFactory());
        }

        public IBusTripsManager CreateBusTripsManager()
        {
            return new BusTripsManager(new DataAccessFactory());
        }

        public IClasssManager CreateClasssManager()
        {
            return new ClasssManager(new DataAccessFactory());
        }

        public IClassroomsManager CreateClassroomsManager()
        {
            return new ClassroomsManager(new DataAccessFactory());
        }

        public ICountrysManager CreateCountrysManager()
        {
            return new CountrysManager(new DataAccessFactory());
        }

        public ICreditCardTypesManager CreateCreditCardTypesManager()
        {
            return new CreditCardTypesManager(new DataAccessFactory());
        }

        public ICurrencysManager CreateCurrencysManager()
        {
            return new CurrencysManager(new DataAccessFactory());
        }

        public ICurriculumsManager CreateCurriculumsManager()
        {
            return new CurriculumsManager(new DataAccessFactory());
        }

        public ICurriculumDepartmentsManager CreateCurriculumDepartmentsManager()
        {
            return new CurriculumDepartmentsManager(new DataAccessFactory());
        }

        public ICustomPropertyTypesManager CreateCustomPropertyTypesManager()
        {
            return new CustomPropertyTypesManager(new DataAccessFactory());
        }

        public IDepartmentsManager CreateDepartmentsManager()
        {
            return new DepartmentsManager(new DataAccessFactory());
        }

        public IDesignationsManager CreateDesignationsManager()
        {
            return new DesignationsManager(new DataAccessFactory());
        }

        public IDesignationTypesManager CreateDesignationTypesManager()
        {
            return new DesignationTypesManager(new DataAccessFactory());
        }

        public IDeviceRegistrarsManager CreateDeviceRegistrarsManager()
        {
            return new DeviceRegistrarsManager(new DataAccessFactory());
        }

        public IDisciplineActionsManager CreateDisciplineActionsManager()
        {
            return new DisciplineActionsManager(new DataAccessFactory());
        }

        public IDiscountsManager CreateDiscountsManager()
        {
            return new DiscountsManager(new DataAccessFactory());
        }

        public IDiscountStudentsManager CreateDiscountStudentsManager()
        {
            return new DiscountStudentsManager(new DataAccessFactory());
        }

        public IDiscountTypesManager CreateDiscountTypesManager()
        {
            return new DiscountTypesManager(new DataAccessFactory());
        }

        public IDocumentsManager CreateDocumentsManager()
        {
            return new DocumentsManager(new DataAccessFactory());
        }

        public IDocumentCustomPropertysManager CreateDocumentCustomPropertysManager()
        {
            return new DocumentCustomPropertysManager(new DataAccessFactory());
        }

        public IDocumentTypesManager CreateDocumentTypesManager()
        {
            return new DocumentTypesManager(new DataAccessFactory());
        }

        public IDocumrntCustomizedPropertysManager CreateDocumrntCustomizedPropertysManager()
        {
            return new DocumrntCustomizedPropertysManager(new DataAccessFactory());
        }

        public IEducationalYearsManager CreateEducationalYearsManager()
        {
            return new EducationalYearsManager(new DataAccessFactory());
        }

        public IExternalGuardiansManager CreateExternalGuardiansManager()
        {
            return new ExternalGuardiansManager(new DataAccessFactory());
        }

        public IExternalGuardStudentsRequestsManager CreateExternalGuardStudentsRequestsManager()
        {
            return new ExternalGuardStudentsRequestsManager(new DataAccessFactory());
        }

        public IExternalOtherStudentDetailsManager CreateExternalOtherStudentDetailsManager()
        {
            return new ExternalOtherStudentDetailsManager(new DataAccessFactory());
        }

        public IExternalStudentsManager CreateExternalStudentsManager()
        {
            return new ExternalStudentsManager(new DataAccessFactory());
        }

        public IExternalStudentAdresssManager CreateExternalStudentAdresssManager()
        {
            return new ExternalStudentAdresssManager(new DataAccessFactory());
        }

        public IExternalStudentGuardDetailsManager CreateExternalStudentGuardDetailsManager()
        {
            return new ExternalStudentGuardDetailsManager(new DataAccessFactory());
        }

        public IExternalStudentSchoolDetailsManager CreateExternalStudentSchoolDetailsManager()
        {
            return new ExternalStudentSchoolDetailsManager(new DataAccessFactory());
        }

        public IFamilysManager CreateFamilysManager()
        {
            return new FamilysManager(new DataAccessFactory());
        }

        public IFeesManager CreateFeesManager()
        {
            return new FeesManager(new DataAccessFactory());
        }

        public IFeeTypesManager CreateFeeTypesManager()
        {
            return new FeeTypesManager(new DataAccessFactory());
        }

        public IFunctionsManager CreateFunctionsManager()
        {
            return new FunctionsManager(new DataAccessFactory());
        }

        public IGroupsManager CreateGroupsManager()
        {
            return new GroupsManager(new DataAccessFactory());
        }

        public IGuardiansManager CreateGuardiansManager()
        {
            return new GuardiansManager(new DataAccessFactory());
        }

        public IGuardianTypesManager CreateGuardianTypesManager()
        {
            return new GuardianTypesManager(new DataAccessFactory());
        }

        public IHeadquarterssManager CreateHeadquarterssManager()
        {
            return new HeadquarterssManager(new DataAccessFactory());
        }

        public IImportanceTypesManager CreateImportanceTypesManager()
        {
            return new ImportanceTypesManager(new DataAccessFactory());
        }

        public IKPIsManager CreateKPIsManager()
        {
            return new KPIsManager(new DataAccessFactory());
        }

        public ILanguagesManager CreateLanguagesManager()
        {
            return new LanguagesManager(new DataAccessFactory());
        }

        public ILiveswithTypesManager CreateLiveswithTypesManager()
        {
            return new LiveswithTypesManager(new DataAccessFactory());
        }

        public ILogsManager CreateLogsManager()
        {
            return new LogsManager(new DataAccessFactory());
        }

        public ILookupValuesManager CreateLookupValuesManager()
        {
            return new LookupValuesManager(new DataAccessFactory());
        }

        public IMailTemplatesManager CreateMailTemplatesManager()
        {
            return new MailTemplatesManager(new DataAccessFactory());
        }

        public IMaintenanceTypesManager CreateMaintenanceTypesManager()
        {
            return new MaintenanceTypesManager(new DataAccessFactory());
        }

        public IManualTimetablesManager CreateManualTimetablesManager()
        {
            return new ManualTimetablesManager(new DataAccessFactory());
        }

        public IMappedFieldsManager CreateMappedFieldsManager()
        {
            return new MappedFieldsManager(new DataAccessFactory());
        }

        public IMaritalStatussManager CreateMaritalStatussManager()
        {
            return new MaritalStatussManager(new DataAccessFactory());
        }

        public IMSTeams_AccountssManager CreateMSTeams_AccountssManager()
        {
            return new MSTeams_AccountssManager(new DataAccessFactory());
        }

        public IMSTeams_SessionAttendessManager CreateMSTeams_SessionAttendessManager()
        {
            return new MSTeams_SessionAttendessManager(new DataAccessFactory());
        }

        public IMSTeams_SessionssManager CreateMSTeams_SessionssManager()
        {
            return new MSTeams_SessionssManager(new DataAccessFactory());
        }

        public INotifiactionStatussManager CreateNotifiactionStatussManager()
        {
            return new NotifiactionStatussManager(new DataAccessFactory());
        }

        public INotificationsManager CreateNotificationsManager()
        {
            return new NotificationsManager(new DataAccessFactory());
        }

        public INotificationsWebsManager CreateNotificationsWebsManager()
        {
            return new NotificationsWebsManager(new DataAccessFactory());
        }

        public IOccasionTypesManager CreateOccasionTypesManager()
        {
            return new OccasionTypesManager(new DataAccessFactory());
        }

        public IOtherStudentDetailsManager CreateOtherStudentDetailsManager()
        {
            return new OtherStudentDetailsManager(new DataAccessFactory());
        }

        public IPaymentMethodsManager CreatePaymentMethodsManager()
        {
            return new PaymentMethodsManager(new DataAccessFactory());
        }

        public IPrivilagesManager CreatePrivilagesManager()
        {
            return new PrivilagesManager(new DataAccessFactory());
        }

        public IQualificationsManager CreateQualificationsManager()
        {
            return new QualificationsManager(new DataAccessFactory());
        }

        public IQuestionBanksManager CreateQuestionBanksManager()
        {
            return new QuestionBanksManager(new DataAccessFactory());
        }

        public IQuestionLevelsManager CreateQuestionLevelsManager()
        {
            return new QuestionLevelsManager(new DataAccessFactory());
        }

        public IQuestionTypesManager CreateQuestionTypesManager()
        {
            return new QuestionTypesManager(new DataAccessFactory());
        }

        public IRecurrenceTypesManager CreateRecurrenceTypesManager()
        {
            return new RecurrenceTypesManager(new DataAccessFactory());
        }

        public IReligionsManager CreateReligionsManager()
        {
            return new ReligionsManager(new DataAccessFactory());
        }

        public IScheduledBusTripsManager CreateScheduledBusTripsManager()
        {
            return new ScheduledBusTripsManager(new DataAccessFactory());
        }

        public IScheduleZonesManager CreateScheduleZonesManager()
        {
            return new ScheduleZonesManager(new DataAccessFactory());
        }

        public ISchoolBranchsManager CreateSchoolBranchsManager()
        {
            return new SchoolBranchsManager(new DataAccessFactory());
        }

        public ISchoolClasssManager CreateSchoolClasssManager()
        {
            return new SchoolClasssManager(new DataAccessFactory());
        }

        public ISchoolSettingsManager CreateSchoolSettingsManager()
        {
            return new SchoolSettingsManager(new DataAccessFactory());
        }

        public ISectionsManager CreateSectionsManager()
        {
            return new SectionsManager(new DataAccessFactory());
        }

        public ISessionsManager CreateSessionsManager()
        {
            return new SessionsManager(new DataAccessFactory());
        }

        public ISessionDistributionsManager CreateSessionDistributionsManager()
        {
            return new SessionDistributionsManager(new DataAccessFactory());
        }

        public ISpecializationsManager CreateSpecializationsManager()
        {
            return new SpecializationsManager(new DataAccessFactory());
        }

        public ISpecialResidenceConditionTypesManager CreateSpecialResidenceConditionTypesManager()
        {
            return new SpecialResidenceConditionTypesManager(new DataAccessFactory());
        }

        public IStaffsManager CreateStaffsManager()
        {
            return new StaffsManager(new DataAccessFactory());
        }

        public IStaffBankDetailsManager CreateStaffBankDetailsManager()
        {
            return new StaffBankDetailsManager(new DataAccessFactory());
        }

        public IStaffContactDetailsManager CreateStaffContactDetailsManager()
        {
            return new StaffContactDetailsManager(new DataAccessFactory());
        }

        public IStaffJobDetailsManager CreateStaffJobDetailsManager()
        {
            return new StaffJobDetailsManager(new DataAccessFactory());
        }

        public IStaffLeafsManager CreateStaffLeafsManager()
        {
            return new StaffLeafsManager(new DataAccessFactory());
        }

        public IStaffSalaryDetailsManager CreateStaffSalaryDetailsManager()
        {
            return new StaffSalaryDetailsManager(new DataAccessFactory());
        }

        public IStudentsManager CreateStudentsManager()
        {
            return new StudentsManager(new DataAccessFactory());
        }

        public IStudent_LoginsManager CreateStudent_LoginsManager()
        {
            return new Student_LoginsManager(new DataAccessFactory());
        }

        public IStudentAdresssManager CreateStudentAdresssManager()
        {
            return new StudentAdresssManager(new DataAccessFactory());
        }

        public IStudentBehaviorsManager CreateStudentBehaviorsManager()
        {
            return new StudentBehaviorsManager(new DataAccessFactory());
        }

        public IStudentDiseassManager CreateStudentDiseassManager()
        {
            return new StudentDiseassManager(new DataAccessFactory());
        }

        public IStudentFeesManager CreateStudentFeesManager()
        {
            return new StudentFeesManager(new DataAccessFactory());
        }

        public IStudentGuardDetailsManager CreateStudentGuardDetailsManager()
        {
            return new StudentGuardDetailsManager(new DataAccessFactory());
        }

        public IStudentHealthsManager CreateStudentHealthsManager()
        {
            return new StudentHealthsManager(new DataAccessFactory());
        }

        public IStudentPaymentsManager CreateStudentPaymentsManager()
        {
            return new StudentPaymentsManager(new DataAccessFactory());
        }

        public IStudentPhysicalStatusManager CreateStudentPhysicalStatusManager()
        {
            return new StudentPhysicalStatusManager(new DataAccessFactory());
        }

        public IStudentResultsManager CreateStudentResultsManager()
        {
            return new StudentResultsManager(new DataAccessFactory());
        }

        public IStudentSchoolDetailsManager CreateStudentSchoolDetailsManager()
        {
            return new StudentSchoolDetailsManager(new DataAccessFactory());
        }

        public IStudentStatusManager CreateStudentStatusManager()
        {
            return new StudentStatusManager(new DataAccessFactory());
        }

        public ISubjectsManager CreateSubjectsManager()
        {
            return new SubjectsManager(new DataAccessFactory());
        }

        public ISystemFieldsManager CreateSystemFieldsManager()
        {
            return new SystemFieldsManager(new DataAccessFactory());
        }

        public ISystemObjectsManager CreateSystemObjectsManager()
        {
            return new SystemObjectsManager(new DataAccessFactory());
        }

        public ISystemSettingsManager CreateSystemSettingsManager()
        {
            return new SystemSettingsManager(new DataAccessFactory());
        }

        public ISystemSettingsperSchoolsManager CreateSystemSettingsperSchoolsManager()
        {
            return new SystemSettingsperSchoolsManager(new DataAccessFactory());
        }

        public ITeachersManager CreateTeachersManager()
        {
            return new TeachersManager(new DataAccessFactory());
        }

        public ITeacherExperiencesManager CreateTeacherExperiencesManager()
        {
            return new TeacherExperiencesManager(new DataAccessFactory());
        }

        public ITeacherSubjectsManager CreateTeacherSubjectsManager()
        {
            return new TeacherSubjectsManager(new DataAccessFactory());
        }

        public ITemplateSettingsManager CreateTemplateSettingsManager()
        {
            return new TemplateSettingsManager(new DataAccessFactory());
        }

        public ITimetablesManager CreateTimetablesManager()
        {
            return new TimetablesManager(new DataAccessFactory());
        }

        public ITimetableConditionsManager CreateTimetableConditionsManager()
        {
            return new TimetableConditionsManager(new DataAccessFactory());
        }

        public ITimetableItemsManager CreateTimetableItemsManager()
        {
            return new TimetableItemsManager(new DataAccessFactory());
        }

        public ITransportCategorysManager CreateTransportCategorysManager()
        {
            return new TransportCategorysManager(new DataAccessFactory());
        }

        public ITransportCategoryTypesManager CreateTransportCategoryTypesManager()
        {
            return new TransportCategoryTypesManager(new DataAccessFactory());
        }

        public ITransportDirectionsManager CreateTransportDirectionsManager()
        {
            return new TransportDirectionsManager(new DataAccessFactory());
        }

        public ITransportTypesManager CreateTransportTypesManager()
        {
            return new TransportTypesManager(new DataAccessFactory());
        }

        public IUsersManager CreateUsersManager()
        {
            return new UsersManager(new DataAccessFactory());
        }

        public IUserTypesManager CreateUserTypesManager()
        {
            return new UserTypesManager(new DataAccessFactory());
        }

        public IVirtualMeetingsManager CreateVirtualMeetingsManager()
        {
            return new VirtualMeetingsManager(new DataAccessFactory());
        }

        public IWebUsersManager CreateWebUsersManager()
        {
            return new WebUsersManager(new DataAccessFactory());
        }

        public IWifesManager CreateWifesManager()
        {
            return new WifesManager(new DataAccessFactory());
        }
    }
}










