using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestToolBox.Model;
using ToolBox.Connections.Database;

namespace TestToolBox
{
    class Program
    {
        private static void Main(string[] args)
        {
            Connection c = new Connection(@"Data Source=AW-BRIAREOS\SQL2016DEV;Initial Catalog=ADO;Integrated Security=True");
            Command cmd = new Command("select * from V_Student;");

            foreach (Student student in c.ExecuteReader(cmd, ToStudent))
                Console.WriteLine($"{student.LastName} {student.FirstName}");

            Console.WriteLine();

            Command cmdSection = new Command("Select Id, SectionName from Section where Id > @Id;");
            cmdSection.AddParameter("Id", 1020);

            foreach (Section section in c.ExecuteReader(cmdSection, delegate (IDataRecord record) { return new Section() { Id = (int)record["Id"], SectionName = (string)record["SectionName"] }; }))
                Console.WriteLine($"{section.Id} {section.SectionName}");
            Console.WriteLine();

            Student newStudent = new Student()
            {
                FirstName = "Thierry",
                LastName = "Morre",
                BirthDate = new DateTime(1974, 6, 5),
                SectionId = 1110,
                YearResult = 12
            };

            Command AddStudent = new Command("AddStudent", true);
            AddStudent.AddParameter("FirstName", newStudent.FirstName);
            AddStudent.AddParameter("LastName", newStudent.LastName);
            AddStudent.AddParameter("BirthDate", newStudent.BirthDate);
            AddStudent.AddParameter("SectionId", newStudent.SectionId);
            AddStudent.AddParameter("YearResult", newStudent.YearResult);

            int Id = (int)c.ExecuteScalar(AddStudent);

            Console.WriteLine(Id);

            Console.ReadLine();
        }

        static Student ToStudent(IDataRecord record)
        {
            return new Student()
            {
                Id = (int)record["Id"],
                FirstName = (string)record["FirstName"],
                LastName = (string)record["LastName"],
                BirthDate = (DateTime)record["BirthDate"],
                YearResult = (int)record["YearResult"],
                SectionId = (int)record["SectionId"],
                Active = (bool)record["Active"]
            };
        }
    }
}
