using System.Data;
using System.IO.Pipes;
using System.Reflection.Metadata;

namespace OG_E_Files;


class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        List<IdentityAccessRecord> l = ReadCsV("Francis Tuttle Identities_Basic.csv");
        Console.WriteLine("Number of Files: " + l.Count());
        var activity = from IdentityAccessRecord in l where IdentityAccessRecord.CloudLifecycleState != true select IdentityAccessRecord;
        Console.WriteLine("Number of inactive Users: " + activity.Count());

        var Departments = from IdentityAccessRecord in l
                          group IdentityAccessRecord by IdentityAccessRecord.Department into departments
                          select new
                          {
                              Name = departments.Key,
                              People = from s in departments where s.CloudLifecycleState != true where s.AccessType != null select s
                          };
        foreach (var name in Departments)
        {
            Console.WriteLine($"Name: {name.Name}");
            Console.WriteLine($"  People: {name.People.Count()}");
        }



        /*  var activity2 = (from poops in activity
                           orderby poops.DisplayName
                           select poops.DisplayName).Distinct();

          foreach (var name in activity2)
          {
              Console.WriteLine(name);
          }
  */
        /*  var peeps = from peels in activity
                      group peels by peels.DisplayName into AccessItems
                      select new
                      {
                          Name = AccessItems.Key,
                          Access = AccessItems
                      };

          foreach (var person in peeps)
          {
              Console.WriteLine($"Name: {person.Name}");
              foreach (var access in person.Access)
              {

                   if (access.AccessSourceName.Length != 0)   Console.WriteLine($"     Source Name: {access.AccessSourceName}");
                  if (access.AccessDisplayName.Length != 0)  Console.WriteLine($"     Description: {access.AccessDisplayName}");

              }
          }
         */


    }


    static List<IdentityAccessRecord> ReadCsV(string fileName)
    {
        using StreamReader s = new StreamReader(fileName);
        s.ReadLine();
        string line = s.ReadLine();

        List<string[]> list = new List<string[]>();
        try
        {

            while (line != null)
            {
                list.Add(line.Split(","));
                line = s.ReadLine();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Oopsie");
        }
        finally { s.Close(); }

        List<IdentityAccessRecord> dict = new List<IdentityAccessRecord>();
        foreach (string[] strArr in list)
        {
            IdentityAccessRecord info = new IdentityAccessRecord(
                strArr[0],
                strArr[1],
                strArr[2],
                strArr[3],
               checkIfActive(strArr[4]),
                strArr[5],
                bool.Parse(strArr[6]),
                strArr[7],
                strArr[8],
                strArr[9],
                strArr[10],
                strArr[11],
                strArr[12],
                strArr[13]);
            dict.Add(info);

        }
        return dict;


    }
    static bool checkIfActive(string s) {
        if(s.Equals("active"))  return true;
        return false;
    }

    // static void display(Dictionary<string, List<Student>> Dict)
    // {
    //     foreach (KeyValuePair<string, List<Student>> studList in Dict)
    //     {
    //         Console.WriteLine($"Student ID: {studList.Value[0].ID}");
    //         Console.WriteLine($"Student Name: {studList.Value[0].Name}");

    //         Console.WriteLine($"{"Date",10} {"Course",10} {"Event",10}");
    //         Console.WriteLine($"{"=====",10} {"=====",10} {"=====",10}");
    //         foreach (Student stud in studList.Value)
    //         {
    //             Console.WriteLine($"{stud.date,10},{stud.Course,10},{stud.Event,10}");

    //         }
    //         Console.WriteLine("______________________");
    //     }

    //     foreach (KeyValuePair<string, List<Student>> studList in Dict)
    //     {
    //         Console.WriteLine($"Student ID: {studList.Value[0].ID}");
    //         Console.WriteLine($"Student Name: {studList.Value[0].Name}");
    //         int totalTardies = 0;
    //         int totalAbsences = 0;
    //         foreach (Student stud in studList.Value)
    //         {
    //             if (stud.Event == "Absent") { totalTardies++; }
    //             if (stud.Event == "Tardy") { totalAbsences++; }

    //         }
    //         Console.WriteLine($"Total Absences: {totalAbsences}");
    //         Console.WriteLine($"Total Tardies: {totalTardies}");
    //         Console.WriteLine("_________________________________");

    //     }
    // }
}
public struct IdentityAccessRecord
{
    // User Core Fields
    public string DisplayName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string WorkEmail { get; set; }
    public bool CloudLifecycleState { get; set; } // Changed to string for "active"
    public string IdentityId { get; set; }
    public bool IsManager { get; set; }
    public string Department { get; set; }
    public string JobTitle { get; set; }
    public string Uid { get; set; }

    // Access Specific Fields
    public string AccessType { get; set; }
    public string AccessSourceName { get; set; }
    public string AccessDisplayName { get; set; }
    public string AccessDescription { get; set; }

    // Constructor
    public IdentityAccessRecord(
        string displayName, string firstName, string lastName, string workEmail, 
        bool cloudLifecycleState, string identityId, bool isManager, string department, 
        string jobTitle, string uid, string accessType, string accessSourceName, 
        string accessDisplayName, string accessDescription)
    {
        this.DisplayName = displayName;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.WorkEmail = workEmail;
        this.CloudLifecycleState = cloudLifecycleState;
        this.IdentityId = identityId;
        this.IsManager = isManager;
        this.Department = department;
        this.JobTitle = jobTitle;
        this.Uid = uid;
        this.AccessType = accessType;
        this.AccessSourceName = accessSourceName;
        this.AccessDisplayName = accessDisplayName;
        this.AccessDescription = accessDescription;
    }
}
