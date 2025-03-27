using Microsoft.AspNetCore.Mvc;
using EmployeeRecord.Models;
using EmployeeRecord.DAL;

namespace EmployeeRecord.Controllers
{
    public class EmployeeRecordController : Controller
    {
        private readonly IEmployeeRecordRepository _EmployeeRecordRepository;

        public EmployeeRecordController(IEmployeeRecordRepository EmployeeRecordRepository)
        {
            _EmployeeRecordRepository = EmployeeRecordRepository;
        }
        public IActionResult Index(string id, string Command, EmployeeRecords ER)
        {
            EmployeeRecords ERS = new EmployeeRecords();
            EmployeeRecords Temp = new EmployeeRecords();

            if (Command == "Save")
            {
                ERS.EmployeeID = ER.EmployeeID;
                ERS.EmployeeName = ER.EmployeeName;
                ERS.EmployeeAddress = ER.EmployeeAddress;
                ERS.EmployeePhone = ER.EmployeePhone;
                ERS.EmployeeEmail = ER.EmployeeEmail;

                if (ERS.EmployeeName != null)
                {
                    _EmployeeRecordRepository.AddEditEmployee(ERS);
                }
            }

            if (Command == "Delete_Single")
            {
                ERS.EmployeeIDs = ER.EmployeeIDs;
                _EmployeeRecordRepository.DeleteEmployee(ER);
            }
            if (Command == "Delete_All")
            {
                ERS.EmployeeIDs = ER.EmployeeIDs;
                _EmployeeRecordRepository.DeleteEmployee(ER);
            }

            ModelState.Clear();
            ViewBag.EmployeeDetailsReport = _EmployeeRecordRepository.ManageEmployee(Temp);

            return View(Temp);
        }

        public JsonResult GetData(string id)
        {
            try
            {
                EmployeeRecords ER = new EmployeeRecords
                {
                    EmployeeID = id
                };

                var employeeRecord = _EmployeeRecordRepository.ManageEmployee(ER).FirstOrDefault();

                if (employeeRecord == null)
                {
                    return Json(new { success = false, message = "Record not found." });
                }

                var data = new
                {
                    EmployeeID = employeeRecord.EmployeeID,
                    EmployeeName = employeeRecord.EmployeeName,
                    EmployeeAddress = employeeRecord.EmployeeAddress,
                    EmployeePhone = employeeRecord.EmployeePhone,
                    EmployeeEmail = employeeRecord.EmployeeEmail
                };

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
