/*
Default code generation for DataAcccessFactory Class ,this code will be overwrited every time the code been generated 
in case to add any custom code please add it in DataAccessFactoryCustom class 
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
public partial class DataAccessFactory : IDataAccessFactory
{


    private AbsenceReasonsRepository _AbsenceReasonRepository;
    public IAbsenceReasonsRepository GetAbsenceReasonsRepository
    {
        get
        {
            if (_AbsenceReasonRepository == null)
                _AbsenceReasonRepository = UnitOfWork.CreateRepository<AbsenceReasonsRepository, AbsenceReason>();

            return _AbsenceReasonRepository;
        }
}

    private AcademicCalendarsRepository _AcademicCalendarRepository;
    public IAcademicCalendarsRepository GetAcademicCalendarsRepository
    {
        get
        {
            if (_AcademicCalendarRepository == null)
                _AcademicCalendarRepository = UnitOfWork.CreateRepository<AcademicCalendarsRepository, AcademicCalendar>();

            return _AcademicCalendarRepository;
        }
}

    private AcademicCalendar1sRepository _AcademicCalendar1Repository;
    public IAcademicCalendar1sRepository GetAcademicCalendar1sRepository
    {
        get
        {
            if (_AcademicCalendar1Repository == null)
                _AcademicCalendar1Repository = UnitOfWork.CreateRepository<AcademicCalendar1sRepository, AcademicCalendar1>();

            return _AcademicCalendar1Repository;
        }
}

    private AccountOwnerTypesRepository _AccountOwnerTypeRepository;
    public IAccountOwnerTypesRepository GetAccountOwnerTypesRepository
    {
        get
        {
            if (_AccountOwnerTypeRepository == null)
                _AccountOwnerTypeRepository = UnitOfWork.CreateRepository<AccountOwnerTypesRepository, AccountOwnerType>();

            return _AccountOwnerTypeRepository;
        }
}

    private AccountsDetailsRepository _AccountsDetailRepository;
    public IAccountsDetailsRepository GetAccountsDetailsRepository
    {
        get
        {
            if (_AccountsDetailRepository == null)
                _AccountsDetailRepository = UnitOfWork.CreateRepository<AccountsDetailsRepository, AccountsDetail>();

            return _AccountsDetailRepository;
        }
}

    private AccountsMastersRepository _AccountsMasterRepository;
    public IAccountsMastersRepository GetAccountsMastersRepository
    {
        get
        {
            if (_AccountsMasterRepository == null)
                _AccountsMasterRepository = UnitOfWork.CreateRepository<AccountsMastersRepository, AccountsMaster>();

            return _AccountsMasterRepository;
        }
}

    private AccountTypesRepository _AccountTypeRepository;
    public IAccountTypesRepository GetAccountTypesRepository
    {
        get
        {
            if (_AccountTypeRepository == null)
                _AccountTypeRepository = UnitOfWork.CreateRepository<AccountTypesRepository, AccountType>();

            return _AccountTypeRepository;
        }
}

    private ActiveInstalltionsRepository _ActiveInstalltionRepository;
    public IActiveInstalltionsRepository GetActiveInstalltionsRepository
    {
        get
        {
            if (_ActiveInstalltionRepository == null)
                _ActiveInstalltionRepository = UnitOfWork.CreateRepository<ActiveInstalltionsRepository, ActiveInstalltion>();

            return _ActiveInstalltionRepository;
        }
}

    private ActiveProductKeysRepository _ActiveProductKeyRepository;
    public IActiveProductKeysRepository GetActiveProductKeysRepository
    {
        get
        {
            if (_ActiveProductKeyRepository == null)
                _ActiveProductKeyRepository = UnitOfWork.CreateRepository<ActiveProductKeysRepository, ActiveProductKey>();

            return _ActiveProductKeyRepository;
        }
}

    private AgeClassificationsRepository _AgeClassificationRepository;
    public IAgeClassificationsRepository GetAgeClassificationsRepository
    {
        get
        {
            if (_AgeClassificationRepository == null)
                _AgeClassificationRepository = UnitOfWork.CreateRepository<AgeClassificationsRepository, AgeClassification>();

            return _AgeClassificationRepository;
        }
}

    private AppraisalsRepository _AppraisalRepository;
    public IAppraisalsRepository GetAppraisalsRepository
    {
        get
        {
            if (_AppraisalRepository == null)
                _AppraisalRepository = UnitOfWork.CreateRepository<AppraisalsRepository, Appraisal>();

            return _AppraisalRepository;
        }
}

    private AppraisalMetricsGroupsRepository _AppraisalMetricsGroupRepository;
    public IAppraisalMetricsGroupsRepository GetAppraisalMetricsGroupsRepository
    {
        get
        {
            if (_AppraisalMetricsGroupRepository == null)
                _AppraisalMetricsGroupRepository = UnitOfWork.CreateRepository<AppraisalMetricsGroupsRepository, AppraisalMetricsGroup>();

            return _AppraisalMetricsGroupRepository;
        }
}

    private AppraisalTypesRepository _AppraisalTypeRepository;
    public IAppraisalTypesRepository GetAppraisalTypesRepository
    {
        get
        {
            if (_AppraisalTypeRepository == null)
                _AppraisalTypeRepository = UnitOfWork.CreateRepository<AppraisalTypesRepository, AppraisalType>();

            return _AppraisalTypeRepository;
        }
}

    private AppraisalValuesRepository _AppraisalValueRepository;
    public IAppraisalValuesRepository GetAppraisalValuesRepository
    {
        get
        {
            if (_AppraisalValueRepository == null)
                _AppraisalValueRepository = UnitOfWork.CreateRepository<AppraisalValuesRepository, AppraisalValue>();

            return _AppraisalValueRepository;
        }
}

    private AreasRepository _AreaRepository;
    public IAreasRepository GetAreasRepository
    {
        get
        {
            if (_AreaRepository == null)
                _AreaRepository = UnitOfWork.CreateRepository<AreasRepository, Area>();

            return _AreaRepository;
        }
}

    private AspNetRolesRepository _AspNetRoleRepository;
    public IAspNetRolesRepository GetAspNetRolesRepository
    {
        get
        {
            if (_AspNetRoleRepository == null)
                _AspNetRoleRepository = UnitOfWork.CreateRepository<AspNetRolesRepository, AspNetRole>();

            return _AspNetRoleRepository;
        }
}

    private AttendancesRepository _AttendanceRepository;
    public IAttendancesRepository GetAttendancesRepository
    {
        get
        {
            if (_AttendanceRepository == null)
                _AttendanceRepository = UnitOfWork.CreateRepository<AttendancesRepository, Attendance>();

            return _AttendanceRepository;
        }
}

    private AttendanceTypesRepository _AttendanceTypeRepository;
    public IAttendanceTypesRepository GetAttendanceTypesRepository
    {
        get
        {
            if (_AttendanceTypeRepository == null)
                _AttendanceTypeRepository = UnitOfWork.CreateRepository<AttendanceTypesRepository, AttendanceType>();

            return _AttendanceTypeRepository;
        }
}

    private AutomaticTimetablesRepository _AutomaticTimetableRepository;
    public IAutomaticTimetablesRepository GetAutomaticTimetablesRepository
    {
        get
        {
            if (_AutomaticTimetableRepository == null)
                _AutomaticTimetableRepository = UnitOfWork.CreateRepository<AutomaticTimetablesRepository, AutomaticTimetable>();

            return _AutomaticTimetableRepository;
        }
}

    private BanksRepository _BankRepository;
    public IBanksRepository GetBanksRepository
    {
        get
        {
            if (_BankRepository == null)
                _BankRepository = UnitOfWork.CreateRepository<BanksRepository, Bank>();

            return _BankRepository;
        }
}

    private BankBranchsRepository _BankBranchRepository;
    public IBankBranchsRepository GetBankBranchsRepository
    {
        get
        {
            if (_BankBranchRepository == null)
                _BankBranchRepository = UnitOfWork.CreateRepository<BankBranchsRepository, BankBranch>();

            return _BankBranchRepository;
        }
}

    private BloodTypesRepository _BloodTypeRepository;
    public IBloodTypesRepository GetBloodTypesRepository
    {
        get
        {
            if (_BloodTypeRepository == null)
                _BloodTypeRepository = UnitOfWork.CreateRepository<BloodTypesRepository, BloodType>();

            return _BloodTypeRepository;
        }
}

    private BooksRepository _BookRepository;
    public IBooksRepository GetBooksRepository
    {
        get
        {
            if (_BookRepository == null)
                _BookRepository = UnitOfWork.CreateRepository<BooksRepository, Book>();

            return _BookRepository;
        }
}

    private BookChaptersRepository _BookChapterRepository;
    public IBookChaptersRepository GetBookChaptersRepository
    {
        get
        {
            if (_BookChapterRepository == null)
                _BookChapterRepository = UnitOfWork.CreateRepository<BookChaptersRepository, BookChapter>();

            return _BookChapterRepository;
        }
}

    private BookUnitsRepository _BookUnitRepository;
    public IBookUnitsRepository GetBookUnitsRepository
    {
        get
        {
            if (_BookUnitRepository == null)
                _BookUnitRepository = UnitOfWork.CreateRepository<BookUnitsRepository, BookUnit>();

            return _BookUnitRepository;
        }
}

    private BusAttendanceSchedulesRepository _BusAttendanceScheduleRepository;
    public IBusAttendanceSchedulesRepository GetBusAttendanceSchedulesRepository
    {
        get
        {
            if (_BusAttendanceScheduleRepository == null)
                _BusAttendanceScheduleRepository = UnitOfWork.CreateRepository<BusAttendanceSchedulesRepository, BusAttendanceSchedule>();

            return _BusAttendanceScheduleRepository;
        }
}

    private BusFuelsRepository _BusFuelRepository;
    public IBusFuelsRepository GetBusFuelsRepository
    {
        get
        {
            if (_BusFuelRepository == null)
                _BusFuelRepository = UnitOfWork.CreateRepository<BusFuelsRepository, BusFuel>();

            return _BusFuelRepository;
        }
}

    private BusInfosRepository _BusInfoRepository;
    public IBusInfosRepository GetBusInfosRepository
    {
        get
        {
            if (_BusInfoRepository == null)
                _BusInfoRepository = UnitOfWork.CreateRepository<BusInfosRepository, BusInfo>();

            return _BusInfoRepository;
        }
}

    private BusMaintenancesRepository _BusMaintenanceRepository;
    public IBusMaintenancesRepository GetBusMaintenancesRepository
    {
        get
        {
            if (_BusMaintenanceRepository == null)
                _BusMaintenanceRepository = UnitOfWork.CreateRepository<BusMaintenancesRepository, BusMaintenance>();

            return _BusMaintenanceRepository;
        }
}

    private BusShiftRotationSchedulesRepository _BusShiftRotationScheduleRepository;
    public IBusShiftRotationSchedulesRepository GetBusShiftRotationSchedulesRepository
    {
        get
        {
            if (_BusShiftRotationScheduleRepository == null)
                _BusShiftRotationScheduleRepository = UnitOfWork.CreateRepository<BusShiftRotationSchedulesRepository, BusShiftRotationSchedule>();

            return _BusShiftRotationScheduleRepository;
        }
}

    private BusShiftRotationScheduleforAutomaticBusSchedulesRepository _BusShiftRotationScheduleforAutomaticBusScheduleRepository;
    public IBusShiftRotationScheduleforAutomaticBusSchedulesRepository GetBusShiftRotationScheduleforAutomaticBusSchedulesRepository
    {
        get
        {
            if (_BusShiftRotationScheduleforAutomaticBusScheduleRepository == null)
                _BusShiftRotationScheduleforAutomaticBusScheduleRepository = UnitOfWork.CreateRepository<BusShiftRotationScheduleforAutomaticBusSchedulesRepository, BusShiftRotationScheduleforAutomaticBusSchedule>();

            return _BusShiftRotationScheduleforAutomaticBusScheduleRepository;
        }
}

    private BusToursRepository _BusTourRepository;
    public IBusToursRepository GetBusToursRepository
    {
        get
        {
            if (_BusTourRepository == null)
                _BusTourRepository = UnitOfWork.CreateRepository<BusToursRepository, BusTour>();

            return _BusTourRepository;
        }
}

    private BusTrackNavigationsRepository _BusTrackNavigationRepository;
    public IBusTrackNavigationsRepository GetBusTrackNavigationsRepository
    {
        get
        {
            if (_BusTrackNavigationRepository == null)
                _BusTrackNavigationRepository = UnitOfWork.CreateRepository<BusTrackNavigationsRepository, BusTrackNavigation>();

            return _BusTrackNavigationRepository;
        }
}

    private BusTripsRepository _BusTripRepository;
    public IBusTripsRepository GetBusTripsRepository
    {
        get
        {
            if (_BusTripRepository == null)
                _BusTripRepository = UnitOfWork.CreateRepository<BusTripsRepository, BusTrip>();

            return _BusTripRepository;
        }
}

    private ClasssRepository _ClassRepository;
    public IClasssRepository GetClasssRepository
    {
        get
        {
            if (_ClassRepository == null)
                _ClassRepository = UnitOfWork.CreateRepository<ClasssRepository, Class>();

            return _ClassRepository;
        }
}

    private ClassroomsRepository _ClassroomRepository;
    public IClassroomsRepository GetClassroomsRepository
    {
        get
        {
            if (_ClassroomRepository == null)
                _ClassroomRepository = UnitOfWork.CreateRepository<ClassroomsRepository, Classroom>();

            return _ClassroomRepository;
        }
}

    private CountrysRepository _CountryRepository;
    public ICountrysRepository GetCountrysRepository
    {
        get
        {
            if (_CountryRepository == null)
                _CountryRepository = UnitOfWork.CreateRepository<CountrysRepository, Country>();

            return _CountryRepository;
        }
}

    private CreditCardTypesRepository _CreditCardTypeRepository;
    public ICreditCardTypesRepository GetCreditCardTypesRepository
    {
        get
        {
            if (_CreditCardTypeRepository == null)
                _CreditCardTypeRepository = UnitOfWork.CreateRepository<CreditCardTypesRepository, CreditCardType>();

            return _CreditCardTypeRepository;
        }
}

    private CurrencysRepository _CurrencyRepository;
    public ICurrencysRepository GetCurrencysRepository
    {
        get
        {
            if (_CurrencyRepository == null)
                _CurrencyRepository = UnitOfWork.CreateRepository<CurrencysRepository, Currency>();

            return _CurrencyRepository;
        }
}

    private CurriculumsRepository _CurriculumRepository;
    public ICurriculumsRepository GetCurriculumsRepository
    {
        get
        {
            if (_CurriculumRepository == null)
                _CurriculumRepository = UnitOfWork.CreateRepository<CurriculumsRepository, Curriculum>();

            return _CurriculumRepository;
        }
}

    private CurriculumDepartmentsRepository _CurriculumDepartmentRepository;
    public ICurriculumDepartmentsRepository GetCurriculumDepartmentsRepository
    {
        get
        {
            if (_CurriculumDepartmentRepository == null)
                _CurriculumDepartmentRepository = UnitOfWork.CreateRepository<CurriculumDepartmentsRepository, CurriculumDepartment>();

            return _CurriculumDepartmentRepository;
        }
}

    private CustomPropertyTypesRepository _CustomPropertyTypeRepository;
    public ICustomPropertyTypesRepository GetCustomPropertyTypesRepository
    {
        get
        {
            if (_CustomPropertyTypeRepository == null)
                _CustomPropertyTypeRepository = UnitOfWork.CreateRepository<CustomPropertyTypesRepository, CustomPropertyType>();

            return _CustomPropertyTypeRepository;
        }
}

    private DepartmentsRepository _DepartmentRepository;
    public IDepartmentsRepository GetDepartmentsRepository
    {
        get
        {
            if (_DepartmentRepository == null)
                _DepartmentRepository = UnitOfWork.CreateRepository<DepartmentsRepository, Department>();

            return _DepartmentRepository;
        }
}

    private DesignationsRepository _DesignationRepository;
    public IDesignationsRepository GetDesignationsRepository
    {
        get
        {
            if (_DesignationRepository == null)
                _DesignationRepository = UnitOfWork.CreateRepository<DesignationsRepository, Designation>();

            return _DesignationRepository;
        }
}

    private DesignationTypesRepository _DesignationTypeRepository;
    public IDesignationTypesRepository GetDesignationTypesRepository
    {
        get
        {
            if (_DesignationTypeRepository == null)
                _DesignationTypeRepository = UnitOfWork.CreateRepository<DesignationTypesRepository, DesignationType>();

            return _DesignationTypeRepository;
        }
}

    private DeviceRegistrarsRepository _DeviceRegistrarRepository;
    public IDeviceRegistrarsRepository GetDeviceRegistrarsRepository
    {
        get
        {
            if (_DeviceRegistrarRepository == null)
                _DeviceRegistrarRepository = UnitOfWork.CreateRepository<DeviceRegistrarsRepository, DeviceRegistrar>();

            return _DeviceRegistrarRepository;
        }
}

    private DisciplineActionsRepository _DisciplineActionRepository;
    public IDisciplineActionsRepository GetDisciplineActionsRepository
    {
        get
        {
            if (_DisciplineActionRepository == null)
                _DisciplineActionRepository = UnitOfWork.CreateRepository<DisciplineActionsRepository, DisciplineAction>();

            return _DisciplineActionRepository;
        }
}

    private DiscountsRepository _DiscountRepository;
    public IDiscountsRepository GetDiscountsRepository
    {
        get
        {
            if (_DiscountRepository == null)
                _DiscountRepository = UnitOfWork.CreateRepository<DiscountsRepository, Discount>();

            return _DiscountRepository;
        }
}

    private DiscountStudentsRepository _DiscountStudentRepository;
    public IDiscountStudentsRepository GetDiscountStudentsRepository
    {
        get
        {
            if (_DiscountStudentRepository == null)
                _DiscountStudentRepository = UnitOfWork.CreateRepository<DiscountStudentsRepository, DiscountStudent>();

            return _DiscountStudentRepository;
        }
}

    private DiscountTypesRepository _DiscountTypeRepository;
    public IDiscountTypesRepository GetDiscountTypesRepository
    {
        get
        {
            if (_DiscountTypeRepository == null)
                _DiscountTypeRepository = UnitOfWork.CreateRepository<DiscountTypesRepository, DiscountType>();

            return _DiscountTypeRepository;
        }
}

    private DocumentsRepository _DocumentRepository;
    public IDocumentsRepository GetDocumentsRepository
    {
        get
        {
            if (_DocumentRepository == null)
                _DocumentRepository = UnitOfWork.CreateRepository<DocumentsRepository, Document>();

            return _DocumentRepository;
        }
}

    private DocumentCustomPropertysRepository _DocumentCustomPropertyRepository;
    public IDocumentCustomPropertysRepository GetDocumentCustomPropertysRepository
    {
        get
        {
            if (_DocumentCustomPropertyRepository == null)
                _DocumentCustomPropertyRepository = UnitOfWork.CreateRepository<DocumentCustomPropertysRepository, DocumentCustomProperty>();

            return _DocumentCustomPropertyRepository;
        }
}

    private DocumentTypesRepository _DocumentTypeRepository;
    public IDocumentTypesRepository GetDocumentTypesRepository
    {
        get
        {
            if (_DocumentTypeRepository == null)
                _DocumentTypeRepository = UnitOfWork.CreateRepository<DocumentTypesRepository, DocumentType>();

            return _DocumentTypeRepository;
        }
}

    private DocumrntCustomizedPropertysRepository _DocumrntCustomizedPropertyRepository;
    public IDocumrntCustomizedPropertysRepository GetDocumrntCustomizedPropertysRepository
    {
        get
        {
            if (_DocumrntCustomizedPropertyRepository == null)
                _DocumrntCustomizedPropertyRepository = UnitOfWork.CreateRepository<DocumrntCustomizedPropertysRepository, DocumrntCustomizedProperty>();

            return _DocumrntCustomizedPropertyRepository;
        }
}

    private EducationalYearsRepository _EducationalYearRepository;
    public IEducationalYearsRepository GetEducationalYearsRepository
    {
        get
        {
            if (_EducationalYearRepository == null)
                _EducationalYearRepository = UnitOfWork.CreateRepository<EducationalYearsRepository, EducationalYear>();

            return _EducationalYearRepository;
        }
}

    private ExternalGuardiansRepository _ExternalGuardianRepository;
    public IExternalGuardiansRepository GetExternalGuardiansRepository
    {
        get
        {
            if (_ExternalGuardianRepository == null)
                _ExternalGuardianRepository = UnitOfWork.CreateRepository<ExternalGuardiansRepository, ExternalGuardian>();

            return _ExternalGuardianRepository;
        }
}

    private ExternalGuardStudentsRequestsRepository _ExternalGuardStudentsRequestRepository;
    public IExternalGuardStudentsRequestsRepository GetExternalGuardStudentsRequestsRepository
    {
        get
        {
            if (_ExternalGuardStudentsRequestRepository == null)
                _ExternalGuardStudentsRequestRepository = UnitOfWork.CreateRepository<ExternalGuardStudentsRequestsRepository, ExternalGuardStudentsRequest>();

            return _ExternalGuardStudentsRequestRepository;
        }
}

    private ExternalOtherStudentDetailsRepository _ExternalOtherStudentDetailRepository;
    public IExternalOtherStudentDetailsRepository GetExternalOtherStudentDetailsRepository
    {
        get
        {
            if (_ExternalOtherStudentDetailRepository == null)
                _ExternalOtherStudentDetailRepository = UnitOfWork.CreateRepository<ExternalOtherStudentDetailsRepository, ExternalOtherStudentDetail>();

            return _ExternalOtherStudentDetailRepository;
        }
}

    private ExternalStudentsRepository _ExternalStudentRepository;
    public IExternalStudentsRepository GetExternalStudentsRepository
    {
        get
        {
            if (_ExternalStudentRepository == null)
                _ExternalStudentRepository = UnitOfWork.CreateRepository<ExternalStudentsRepository, ExternalStudent>();

            return _ExternalStudentRepository;
        }
}

    private ExternalStudentAdresssRepository _ExternalStudentAdressRepository;
    public IExternalStudentAdresssRepository GetExternalStudentAdresssRepository
    {
        get
        {
            if (_ExternalStudentAdressRepository == null)
                _ExternalStudentAdressRepository = UnitOfWork.CreateRepository<ExternalStudentAdresssRepository, ExternalStudentAdress>();

            return _ExternalStudentAdressRepository;
        }
}

    private ExternalStudentGuardDetailsRepository _ExternalStudentGuardDetailRepository;
    public IExternalStudentGuardDetailsRepository GetExternalStudentGuardDetailsRepository
    {
        get
        {
            if (_ExternalStudentGuardDetailRepository == null)
                _ExternalStudentGuardDetailRepository = UnitOfWork.CreateRepository<ExternalStudentGuardDetailsRepository, ExternalStudentGuardDetail>();

            return _ExternalStudentGuardDetailRepository;
        }
}

    private ExternalStudentSchoolDetailsRepository _ExternalStudentSchoolDetailRepository;
    public IExternalStudentSchoolDetailsRepository GetExternalStudentSchoolDetailsRepository
    {
        get
        {
            if (_ExternalStudentSchoolDetailRepository == null)
                _ExternalStudentSchoolDetailRepository = UnitOfWork.CreateRepository<ExternalStudentSchoolDetailsRepository, ExternalStudentSchoolDetail>();

            return _ExternalStudentSchoolDetailRepository;
        }
}

    private FamilysRepository _FamilyRepository;
    public IFamilysRepository GetFamilysRepository
    {
        get
        {
            if (_FamilyRepository == null)
                _FamilyRepository = UnitOfWork.CreateRepository<FamilysRepository, Family>();

            return _FamilyRepository;
        }
}

    private FeesRepository _FeeRepository;
    public IFeesRepository GetFeesRepository
    {
        get
        {
            if (_FeeRepository == null)
                _FeeRepository = UnitOfWork.CreateRepository<FeesRepository, Fee>();

            return _FeeRepository;
        }
}

    private FeeTypesRepository _FeeTypeRepository;
    public IFeeTypesRepository GetFeeTypesRepository
    {
        get
        {
            if (_FeeTypeRepository == null)
                _FeeTypeRepository = UnitOfWork.CreateRepository<FeeTypesRepository, FeeType>();

            return _FeeTypeRepository;
        }
}

    private FunctionsRepository _FunctionRepository;
    public IFunctionsRepository GetFunctionsRepository
    {
        get
        {
            if (_FunctionRepository == null)
                _FunctionRepository = UnitOfWork.CreateRepository<FunctionsRepository, Function>();

            return _FunctionRepository;
        }
}

    private GroupsRepository _GroupRepository;
    public IGroupsRepository GetGroupsRepository
    {
        get
        {
            if (_GroupRepository == null)
                _GroupRepository = UnitOfWork.CreateRepository<GroupsRepository, Group>();

            return _GroupRepository;
        }
}

    private GuardiansRepository _GuardianRepository;
    public IGuardiansRepository GetGuardiansRepository
    {
        get
        {
            if (_GuardianRepository == null)
                _GuardianRepository = UnitOfWork.CreateRepository<GuardiansRepository, Guardian>();

            return _GuardianRepository;
        }
}

    private GuardianTypesRepository _GuardianTypeRepository;
    public IGuardianTypesRepository GetGuardianTypesRepository
    {
        get
        {
            if (_GuardianTypeRepository == null)
                _GuardianTypeRepository = UnitOfWork.CreateRepository<GuardianTypesRepository, GuardianType>();

            return _GuardianTypeRepository;
        }
}

    private HeadquarterssRepository _HeadquartersRepository;
    public IHeadquarterssRepository GetHeadquarterssRepository
    {
        get
        {
            if (_HeadquartersRepository == null)
                _HeadquartersRepository = UnitOfWork.CreateRepository<HeadquarterssRepository, Headquarters>();

            return _HeadquartersRepository;
        }
}

    private ImportanceTypesRepository _ImportanceTypeRepository;
    public IImportanceTypesRepository GetImportanceTypesRepository
    {
        get
        {
            if (_ImportanceTypeRepository == null)
                _ImportanceTypeRepository = UnitOfWork.CreateRepository<ImportanceTypesRepository, ImportanceType>();

            return _ImportanceTypeRepository;
        }
}

    private KPIsRepository _KPIRepository;
    public IKPIsRepository GetKPIsRepository
    {
        get
        {
            if (_KPIRepository == null)
                _KPIRepository = UnitOfWork.CreateRepository<KPIsRepository, KPI>();

            return _KPIRepository;
        }
}

    private LanguagesRepository _LanguageRepository;
    public ILanguagesRepository GetLanguagesRepository
    {
        get
        {
            if (_LanguageRepository == null)
                _LanguageRepository = UnitOfWork.CreateRepository<LanguagesRepository, Language>();

            return _LanguageRepository;
        }
}

    private LiveswithTypesRepository _LiveswithTypeRepository;
    public ILiveswithTypesRepository GetLiveswithTypesRepository
    {
        get
        {
            if (_LiveswithTypeRepository == null)
                _LiveswithTypeRepository = UnitOfWork.CreateRepository<LiveswithTypesRepository, LiveswithType>();

            return _LiveswithTypeRepository;
        }
}

    private LogsRepository _LogRepository;
    public ILogsRepository GetLogsRepository
    {
        get
        {
            if (_LogRepository == null)
                _LogRepository = UnitOfWork.CreateRepository<LogsRepository, Log>();

            return _LogRepository;
        }
}

    private LookupValuesRepository _LookupValueRepository;
    public ILookupValuesRepository GetLookupValuesRepository
    {
        get
        {
            if (_LookupValueRepository == null)
                _LookupValueRepository = UnitOfWork.CreateRepository<LookupValuesRepository, LookupValue>();

            return _LookupValueRepository;
        }
}

    private MailTemplatesRepository _MailTemplateRepository;
    public IMailTemplatesRepository GetMailTemplatesRepository
    {
        get
        {
            if (_MailTemplateRepository == null)
                _MailTemplateRepository = UnitOfWork.CreateRepository<MailTemplatesRepository, MailTemplate>();

            return _MailTemplateRepository;
        }
}

    private MaintenanceTypesRepository _MaintenanceTypeRepository;
    public IMaintenanceTypesRepository GetMaintenanceTypesRepository
    {
        get
        {
            if (_MaintenanceTypeRepository == null)
                _MaintenanceTypeRepository = UnitOfWork.CreateRepository<MaintenanceTypesRepository, MaintenanceType>();

            return _MaintenanceTypeRepository;
        }
}

    private ManualTimetablesRepository _ManualTimetableRepository;
    public IManualTimetablesRepository GetManualTimetablesRepository
    {
        get
        {
            if (_ManualTimetableRepository == null)
                _ManualTimetableRepository = UnitOfWork.CreateRepository<ManualTimetablesRepository, ManualTimetable>();

            return _ManualTimetableRepository;
        }
}

    private MappedFieldsRepository _MappedFieldRepository;
    public IMappedFieldsRepository GetMappedFieldsRepository
    {
        get
        {
            if (_MappedFieldRepository == null)
                _MappedFieldRepository = UnitOfWork.CreateRepository<MappedFieldsRepository, MappedField>();

            return _MappedFieldRepository;
        }
}

    private MaritalStatussRepository _MaritalStatusRepository;
    public IMaritalStatussRepository GetMaritalStatussRepository
    {
        get
        {
            if (_MaritalStatusRepository == null)
                _MaritalStatusRepository = UnitOfWork.CreateRepository<MaritalStatussRepository, MaritalStatus>();

            return _MaritalStatusRepository;
        }
}

    private MSTeams_AccountssRepository _MSTeams_AccountsRepository;
    public IMSTeams_AccountssRepository GetMSTeams_AccountssRepository
    {
        get
        {
            if (_MSTeams_AccountsRepository == null)
                _MSTeams_AccountsRepository = UnitOfWork.CreateRepository<MSTeams_AccountssRepository, MSTeams_Accounts>();

            return _MSTeams_AccountsRepository;
        }
}

    private MSTeams_SessionAttendessRepository _MSTeams_SessionAttendesRepository;
    public IMSTeams_SessionAttendessRepository GetMSTeams_SessionAttendessRepository
    {
        get
        {
            if (_MSTeams_SessionAttendesRepository == null)
                _MSTeams_SessionAttendesRepository = UnitOfWork.CreateRepository<MSTeams_SessionAttendessRepository, MSTeams_SessionAttendes>();

            return _MSTeams_SessionAttendesRepository;
        }
}

    private MSTeams_SessionssRepository _MSTeams_SessionsRepository;
    public IMSTeams_SessionssRepository GetMSTeams_SessionssRepository
    {
        get
        {
            if (_MSTeams_SessionsRepository == null)
                _MSTeams_SessionsRepository = UnitOfWork.CreateRepository<MSTeams_SessionssRepository, MSTeams_Sessions>();

            return _MSTeams_SessionsRepository;
        }
}

    private NotifiactionStatussRepository _NotifiactionStatusRepository;
    public INotifiactionStatussRepository GetNotifiactionStatussRepository
    {
        get
        {
            if (_NotifiactionStatusRepository == null)
                _NotifiactionStatusRepository = UnitOfWork.CreateRepository<NotifiactionStatussRepository, NotifiactionStatus>();

            return _NotifiactionStatusRepository;
        }
}

    private NotificationsRepository _NotificationRepository;
    public INotificationsRepository GetNotificationsRepository
    {
        get
        {
            if (_NotificationRepository == null)
                _NotificationRepository = UnitOfWork.CreateRepository<NotificationsRepository, Notification>();

            return _NotificationRepository;
        }
}

    private NotificationsWebsRepository _NotificationsWebRepository;
    public INotificationsWebsRepository GetNotificationsWebsRepository
    {
        get
        {
            if (_NotificationsWebRepository == null)
                _NotificationsWebRepository = UnitOfWork.CreateRepository<NotificationsWebsRepository, NotificationsWeb>();

            return _NotificationsWebRepository;
        }
}

    private OccasionTypesRepository _OccasionTypeRepository;
    public IOccasionTypesRepository GetOccasionTypesRepository
    {
        get
        {
            if (_OccasionTypeRepository == null)
                _OccasionTypeRepository = UnitOfWork.CreateRepository<OccasionTypesRepository, OccasionType>();

            return _OccasionTypeRepository;
        }
}

    private OtherStudentDetailsRepository _OtherStudentDetailRepository;
    public IOtherStudentDetailsRepository GetOtherStudentDetailsRepository
    {
        get
        {
            if (_OtherStudentDetailRepository == null)
                _OtherStudentDetailRepository = UnitOfWork.CreateRepository<OtherStudentDetailsRepository, OtherStudentDetail>();

            return _OtherStudentDetailRepository;
        }
}

    private PaymentMethodsRepository _PaymentMethodRepository;
    public IPaymentMethodsRepository GetPaymentMethodsRepository
    {
        get
        {
            if (_PaymentMethodRepository == null)
                _PaymentMethodRepository = UnitOfWork.CreateRepository<PaymentMethodsRepository, PaymentMethod>();

            return _PaymentMethodRepository;
        }
}

    private PrivilagesRepository _PrivilageRepository;
    public IPrivilagesRepository GetPrivilagesRepository
    {
        get
        {
            if (_PrivilageRepository == null)
                _PrivilageRepository = UnitOfWork.CreateRepository<PrivilagesRepository, Privilage>();

            return _PrivilageRepository;
        }
}

    private QualificationsRepository _QualificationRepository;
    public IQualificationsRepository GetQualificationsRepository
    {
        get
        {
            if (_QualificationRepository == null)
                _QualificationRepository = UnitOfWork.CreateRepository<QualificationsRepository, Qualification>();

            return _QualificationRepository;
        }
}

    private QuestionBanksRepository _QuestionBankRepository;
    public IQuestionBanksRepository GetQuestionBanksRepository
    {
        get
        {
            if (_QuestionBankRepository == null)
                _QuestionBankRepository = UnitOfWork.CreateRepository<QuestionBanksRepository, QuestionBank>();

            return _QuestionBankRepository;
        }
}

    private QuestionLevelsRepository _QuestionLevelRepository;
    public IQuestionLevelsRepository GetQuestionLevelsRepository
    {
        get
        {
            if (_QuestionLevelRepository == null)
                _QuestionLevelRepository = UnitOfWork.CreateRepository<QuestionLevelsRepository, QuestionLevel>();

            return _QuestionLevelRepository;
        }
}

    private QuestionTypesRepository _QuestionTypeRepository;
    public IQuestionTypesRepository GetQuestionTypesRepository
    {
        get
        {
            if (_QuestionTypeRepository == null)
                _QuestionTypeRepository = UnitOfWork.CreateRepository<QuestionTypesRepository, QuestionType>();

            return _QuestionTypeRepository;
        }
}

    private RecurrenceTypesRepository _RecurrenceTypeRepository;
    public IRecurrenceTypesRepository GetRecurrenceTypesRepository
    {
        get
        {
            if (_RecurrenceTypeRepository == null)
                _RecurrenceTypeRepository = UnitOfWork.CreateRepository<RecurrenceTypesRepository, RecurrenceType>();

            return _RecurrenceTypeRepository;
        }
}

    private ReligionsRepository _ReligionRepository;
    public IReligionsRepository GetReligionsRepository
    {
        get
        {
            if (_ReligionRepository == null)
                _ReligionRepository = UnitOfWork.CreateRepository<ReligionsRepository, Religion>();

            return _ReligionRepository;
        }
}

    private ScheduledBusTripsRepository _ScheduledBusTripRepository;
    public IScheduledBusTripsRepository GetScheduledBusTripsRepository
    {
        get
        {
            if (_ScheduledBusTripRepository == null)
                _ScheduledBusTripRepository = UnitOfWork.CreateRepository<ScheduledBusTripsRepository, ScheduledBusTrip>();

            return _ScheduledBusTripRepository;
        }
}

    private ScheduleZonesRepository _ScheduleZoneRepository;
    public IScheduleZonesRepository GetScheduleZonesRepository
    {
        get
        {
            if (_ScheduleZoneRepository == null)
                _ScheduleZoneRepository = UnitOfWork.CreateRepository<ScheduleZonesRepository, ScheduleZone>();

            return _ScheduleZoneRepository;
        }
}

    private SchoolBranchsRepository _SchoolBranchRepository;
    public ISchoolBranchsRepository GetSchoolBranchsRepository
    {
        get
        {
            if (_SchoolBranchRepository == null)
                _SchoolBranchRepository = UnitOfWork.CreateRepository<SchoolBranchsRepository, SchoolBranch>();

            return _SchoolBranchRepository;
        }
}

    private SchoolClasssRepository _SchoolClassRepository;
    public ISchoolClasssRepository GetSchoolClasssRepository
    {
        get
        {
            if (_SchoolClassRepository == null)
                _SchoolClassRepository = UnitOfWork.CreateRepository<SchoolClasssRepository, SchoolClass>();

            return _SchoolClassRepository;
        }
}

    private SchoolSettingsRepository _SchoolSettingRepository;
    public ISchoolSettingsRepository GetSchoolSettingsRepository
    {
        get
        {
            if (_SchoolSettingRepository == null)
                _SchoolSettingRepository = UnitOfWork.CreateRepository<SchoolSettingsRepository, SchoolSetting>();

            return _SchoolSettingRepository;
        }
}

    private SectionsRepository _SectionRepository;
    public ISectionsRepository GetSectionsRepository
    {
        get
        {
            if (_SectionRepository == null)
                _SectionRepository = UnitOfWork.CreateRepository<SectionsRepository, Section>();

            return _SectionRepository;
        }
}

    private SessionsRepository _SessionRepository;
    public ISessionsRepository GetSessionsRepository
    {
        get
        {
            if (_SessionRepository == null)
                _SessionRepository = UnitOfWork.CreateRepository<SessionsRepository, Session>();

            return _SessionRepository;
        }
}

    private SessionDistributionsRepository _SessionDistributionRepository;
    public ISessionDistributionsRepository GetSessionDistributionsRepository
    {
        get
        {
            if (_SessionDistributionRepository == null)
                _SessionDistributionRepository = UnitOfWork.CreateRepository<SessionDistributionsRepository, SessionDistribution>();

            return _SessionDistributionRepository;
        }
}

    private SpecializationsRepository _SpecializationRepository;
    public ISpecializationsRepository GetSpecializationsRepository
    {
        get
        {
            if (_SpecializationRepository == null)
                _SpecializationRepository = UnitOfWork.CreateRepository<SpecializationsRepository, Specialization>();

            return _SpecializationRepository;
        }
}

    private SpecialResidenceConditionTypesRepository _SpecialResidenceConditionTypeRepository;
    public ISpecialResidenceConditionTypesRepository GetSpecialResidenceConditionTypesRepository
    {
        get
        {
            if (_SpecialResidenceConditionTypeRepository == null)
                _SpecialResidenceConditionTypeRepository = UnitOfWork.CreateRepository<SpecialResidenceConditionTypesRepository, SpecialResidenceConditionType>();

            return _SpecialResidenceConditionTypeRepository;
        }
}

    private StaffsRepository _StaffRepository;
    public IStaffsRepository GetStaffsRepository
    {
        get
        {
            if (_StaffRepository == null)
                _StaffRepository = UnitOfWork.CreateRepository<StaffsRepository, Staff>();

            return _StaffRepository;
        }
}

    private StaffBankDetailsRepository _StaffBankDetailRepository;
    public IStaffBankDetailsRepository GetStaffBankDetailsRepository
    {
        get
        {
            if (_StaffBankDetailRepository == null)
                _StaffBankDetailRepository = UnitOfWork.CreateRepository<StaffBankDetailsRepository, StaffBankDetail>();

            return _StaffBankDetailRepository;
        }
}

    private StaffContactDetailsRepository _StaffContactDetailRepository;
    public IStaffContactDetailsRepository GetStaffContactDetailsRepository
    {
        get
        {
            if (_StaffContactDetailRepository == null)
                _StaffContactDetailRepository = UnitOfWork.CreateRepository<StaffContactDetailsRepository, StaffContactDetail>();

            return _StaffContactDetailRepository;
        }
}

    private StaffJobDetailsRepository _StaffJobDetailRepository;
    public IStaffJobDetailsRepository GetStaffJobDetailsRepository
    {
        get
        {
            if (_StaffJobDetailRepository == null)
                _StaffJobDetailRepository = UnitOfWork.CreateRepository<StaffJobDetailsRepository, StaffJobDetail>();

            return _StaffJobDetailRepository;
        }
}

    private StaffLeafsRepository _StaffLeafRepository;
    public IStaffLeafsRepository GetStaffLeafsRepository
    {
        get
        {
            if (_StaffLeafRepository == null)
                _StaffLeafRepository = UnitOfWork.CreateRepository<StaffLeafsRepository, StaffLeaf>();

            return _StaffLeafRepository;
        }
}

    private StaffSalaryDetailsRepository _StaffSalaryDetailRepository;
    public IStaffSalaryDetailsRepository GetStaffSalaryDetailsRepository
    {
        get
        {
            if (_StaffSalaryDetailRepository == null)
                _StaffSalaryDetailRepository = UnitOfWork.CreateRepository<StaffSalaryDetailsRepository, StaffSalaryDetail>();

            return _StaffSalaryDetailRepository;
        }
}

    private StudentsRepository _StudentRepository;
    public IStudentsRepository GetStudentsRepository
    {
        get
        {
            if (_StudentRepository == null)
                _StudentRepository = UnitOfWork.CreateRepository<StudentsRepository, Student>();

            return _StudentRepository;
        }
}

    private Student_LoginsRepository _Student_LoginRepository;
    public IStudent_LoginsRepository GetStudent_LoginsRepository
    {
        get
        {
            if (_Student_LoginRepository == null)
                _Student_LoginRepository = UnitOfWork.CreateRepository<Student_LoginsRepository, Student_Login>();

            return _Student_LoginRepository;
        }
}

    private StudentAdresssRepository _StudentAdressRepository;
    public IStudentAdresssRepository GetStudentAdresssRepository
    {
        get
        {
            if (_StudentAdressRepository == null)
                _StudentAdressRepository = UnitOfWork.CreateRepository<StudentAdresssRepository, StudentAdress>();

            return _StudentAdressRepository;
        }
}

    private StudentBehaviorsRepository _StudentBehaviorRepository;
    public IStudentBehaviorsRepository GetStudentBehaviorsRepository
    {
        get
        {
            if (_StudentBehaviorRepository == null)
                _StudentBehaviorRepository = UnitOfWork.CreateRepository<StudentBehaviorsRepository, StudentBehavior>();

            return _StudentBehaviorRepository;
        }
}

    private StudentDiseassRepository _StudentDiseasRepository;
    public IStudentDiseassRepository GetStudentDiseassRepository
    {
        get
        {
            if (_StudentDiseasRepository == null)
                _StudentDiseasRepository = UnitOfWork.CreateRepository<StudentDiseassRepository, StudentDiseas>();

            return _StudentDiseasRepository;
        }
}

    private StudentFeesRepository _StudentFeeRepository;
    public IStudentFeesRepository GetStudentFeesRepository
    {
        get
        {
            if (_StudentFeeRepository == null)
                _StudentFeeRepository = UnitOfWork.CreateRepository<StudentFeesRepository, StudentFee>();

            return _StudentFeeRepository;
        }
}

    private StudentGuardDetailsRepository _StudentGuardDetailRepository;
    public IStudentGuardDetailsRepository GetStudentGuardDetailsRepository
    {
        get
        {
            if (_StudentGuardDetailRepository == null)
                _StudentGuardDetailRepository = UnitOfWork.CreateRepository<StudentGuardDetailsRepository, StudentGuardDetail>();

            return _StudentGuardDetailRepository;
        }
}

    private StudentHealthsRepository _StudentHealthRepository;
    public IStudentHealthsRepository GetStudentHealthsRepository
    {
        get
        {
            if (_StudentHealthRepository == null)
                _StudentHealthRepository = UnitOfWork.CreateRepository<StudentHealthsRepository, StudentHealth>();

            return _StudentHealthRepository;
        }
}

    private StudentPaymentsRepository _StudentPaymentRepository;
    public IStudentPaymentsRepository GetStudentPaymentsRepository
    {
        get
        {
            if (_StudentPaymentRepository == null)
                _StudentPaymentRepository = UnitOfWork.CreateRepository<StudentPaymentsRepository, StudentPayment>();

            return _StudentPaymentRepository;
        }
}

    private StudentPhysicalStatusRepository _StudentPhysicalStatuRepository;
    public IStudentPhysicalStatusRepository GetStudentPhysicalStatusRepository
    {
        get
        {
            if (_StudentPhysicalStatuRepository == null)
                _StudentPhysicalStatuRepository = UnitOfWork.CreateRepository<StudentPhysicalStatusRepository, StudentPhysicalStatu>();

            return _StudentPhysicalStatuRepository;
        }
}

    private StudentResultsRepository _StudentResultRepository;
    public IStudentResultsRepository GetStudentResultsRepository
    {
        get
        {
            if (_StudentResultRepository == null)
                _StudentResultRepository = UnitOfWork.CreateRepository<StudentResultsRepository, StudentResult>();

            return _StudentResultRepository;
        }
}

    private StudentSchoolDetailsRepository _StudentSchoolDetailRepository;
    public IStudentSchoolDetailsRepository GetStudentSchoolDetailsRepository
    {
        get
        {
            if (_StudentSchoolDetailRepository == null)
                _StudentSchoolDetailRepository = UnitOfWork.CreateRepository<StudentSchoolDetailsRepository, StudentSchoolDetail>();

            return _StudentSchoolDetailRepository;
        }
}

    private StudentStatusRepository _StudentStatuRepository;
    public IStudentStatusRepository GetStudentStatusRepository
    {
        get
        {
            if (_StudentStatuRepository == null)
                _StudentStatuRepository = UnitOfWork.CreateRepository<StudentStatusRepository, StudentStatu>();

            return _StudentStatuRepository;
        }
}

    private SubjectsRepository _SubjectRepository;
    public ISubjectsRepository GetSubjectsRepository
    {
        get
        {
            if (_SubjectRepository == null)
                _SubjectRepository = UnitOfWork.CreateRepository<SubjectsRepository, Subject>();

            return _SubjectRepository;
        }
}

    private SystemFieldsRepository _SystemFieldRepository;
    public ISystemFieldsRepository GetSystemFieldsRepository
    {
        get
        {
            if (_SystemFieldRepository == null)
                _SystemFieldRepository = UnitOfWork.CreateRepository<SystemFieldsRepository, SystemField>();

            return _SystemFieldRepository;
        }
}

    private SystemObjectsRepository _SystemObjectRepository;
    public ISystemObjectsRepository GetSystemObjectsRepository
    {
        get
        {
            if (_SystemObjectRepository == null)
                _SystemObjectRepository = UnitOfWork.CreateRepository<SystemObjectsRepository, SystemObject>();

            return _SystemObjectRepository;
        }
}

    private SystemSettingsRepository _SystemSettingRepository;
    public ISystemSettingsRepository GetSystemSettingsRepository
    {
        get
        {
            if (_SystemSettingRepository == null)
                _SystemSettingRepository = UnitOfWork.CreateRepository<SystemSettingsRepository, SystemSetting>();

            return _SystemSettingRepository;
        }
}

    private SystemSettingsperSchoolsRepository _SystemSettingsperSchoolRepository;
    public ISystemSettingsperSchoolsRepository GetSystemSettingsperSchoolsRepository
    {
        get
        {
            if (_SystemSettingsperSchoolRepository == null)
                _SystemSettingsperSchoolRepository = UnitOfWork.CreateRepository<SystemSettingsperSchoolsRepository, SystemSettingsperSchool>();

            return _SystemSettingsperSchoolRepository;
        }
}

    private TeachersRepository _TeacherRepository;
    public ITeachersRepository GetTeachersRepository
    {
        get
        {
            if (_TeacherRepository == null)
                _TeacherRepository = UnitOfWork.CreateRepository<TeachersRepository, Teacher>();

            return _TeacherRepository;
        }
}

    private TeacherExperiencesRepository _TeacherExperienceRepository;
    public ITeacherExperiencesRepository GetTeacherExperiencesRepository
    {
        get
        {
            if (_TeacherExperienceRepository == null)
                _TeacherExperienceRepository = UnitOfWork.CreateRepository<TeacherExperiencesRepository, TeacherExperience>();

            return _TeacherExperienceRepository;
        }
}

    private TeacherSubjectsRepository _TeacherSubjectRepository;
    public ITeacherSubjectsRepository GetTeacherSubjectsRepository
    {
        get
        {
            if (_TeacherSubjectRepository == null)
                _TeacherSubjectRepository = UnitOfWork.CreateRepository<TeacherSubjectsRepository, TeacherSubject>();

            return _TeacherSubjectRepository;
        }
}

    private TemplateSettingsRepository _TemplateSettingRepository;
    public ITemplateSettingsRepository GetTemplateSettingsRepository
    {
        get
        {
            if (_TemplateSettingRepository == null)
                _TemplateSettingRepository = UnitOfWork.CreateRepository<TemplateSettingsRepository, TemplateSetting>();

            return _TemplateSettingRepository;
        }
}

    private TimetablesRepository _TimetableRepository;
    public ITimetablesRepository GetTimetablesRepository
    {
        get
        {
            if (_TimetableRepository == null)
                _TimetableRepository = UnitOfWork.CreateRepository<TimetablesRepository, Timetable>();

            return _TimetableRepository;
        }
}

    private TimetableConditionsRepository _TimetableConditionRepository;
    public ITimetableConditionsRepository GetTimetableConditionsRepository
    {
        get
        {
            if (_TimetableConditionRepository == null)
                _TimetableConditionRepository = UnitOfWork.CreateRepository<TimetableConditionsRepository, TimetableCondition>();

            return _TimetableConditionRepository;
        }
}

    private TimetableItemsRepository _TimetableItemRepository;
    public ITimetableItemsRepository GetTimetableItemsRepository
    {
        get
        {
            if (_TimetableItemRepository == null)
                _TimetableItemRepository = UnitOfWork.CreateRepository<TimetableItemsRepository, TimetableItem>();

            return _TimetableItemRepository;
        }
}

    private TransportCategorysRepository _TransportCategoryRepository;
    public ITransportCategorysRepository GetTransportCategorysRepository
    {
        get
        {
            if (_TransportCategoryRepository == null)
                _TransportCategoryRepository = UnitOfWork.CreateRepository<TransportCategorysRepository, TransportCategory>();

            return _TransportCategoryRepository;
        }
}

    private TransportCategoryTypesRepository _TransportCategoryTypeRepository;
    public ITransportCategoryTypesRepository GetTransportCategoryTypesRepository
    {
        get
        {
            if (_TransportCategoryTypeRepository == null)
                _TransportCategoryTypeRepository = UnitOfWork.CreateRepository<TransportCategoryTypesRepository, TransportCategoryType>();

            return _TransportCategoryTypeRepository;
        }
}

    private TransportDirectionsRepository _TransportDirectionRepository;
    public ITransportDirectionsRepository GetTransportDirectionsRepository
    {
        get
        {
            if (_TransportDirectionRepository == null)
                _TransportDirectionRepository = UnitOfWork.CreateRepository<TransportDirectionsRepository, TransportDirection>();

            return _TransportDirectionRepository;
        }
}

    private TransportTypesRepository _TransportTypeRepository;
    public ITransportTypesRepository GetTransportTypesRepository
    {
        get
        {
            if (_TransportTypeRepository == null)
                _TransportTypeRepository = UnitOfWork.CreateRepository<TransportTypesRepository, TransportType>();

            return _TransportTypeRepository;
        }
}

    private UsersRepository _UserRepository;
    public IUsersRepository GetUsersRepository
    {
        get
        {
            if (_UserRepository == null)
                _UserRepository = UnitOfWork.CreateRepository<UsersRepository, User>();

            return _UserRepository;
        }
}

    private UserTypesRepository _UserTypeRepository;
    public IUserTypesRepository GetUserTypesRepository
    {
        get
        {
            if (_UserTypeRepository == null)
                _UserTypeRepository = UnitOfWork.CreateRepository<UserTypesRepository, UserType>();

            return _UserTypeRepository;
        }
}

    private VirtualMeetingsRepository _VirtualMeetingRepository;
    public IVirtualMeetingsRepository GetVirtualMeetingsRepository
    {
        get
        {
            if (_VirtualMeetingRepository == null)
                _VirtualMeetingRepository = UnitOfWork.CreateRepository<VirtualMeetingsRepository, VirtualMeeting>();

            return _VirtualMeetingRepository;
        }
}

    private WebUsersRepository _WebUserRepository;
    public IWebUsersRepository GetWebUsersRepository
    {
        get
        {
            if (_WebUserRepository == null)
                _WebUserRepository = UnitOfWork.CreateRepository<WebUsersRepository, WebUser>();

            return _WebUserRepository;
        }
}

    private WifesRepository _WifeRepository;
    public IWifesRepository GetWifesRepository
    {
        get
        {
            if (_WifeRepository == null)
                _WifeRepository = UnitOfWork.CreateRepository<WifesRepository, Wife>();

            return _WifeRepository;
        }
}
}
}

