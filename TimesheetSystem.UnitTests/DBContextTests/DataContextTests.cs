using Microsoft.EntityFrameworkCore;
using TimesheetSystem.Domain.Entities;
using TimesheetSystem.Infrastructure;

namespace TimesheetSystem.UnitTests.DBContextTests;

[TestFixture] 
public class TimesheetDbContextTests
{
    private TimesheetDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<TimesheetDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name here
            .Options;
        _context = new TimesheetDbContext(options);
    }
    [TearDown] 
    
    public void TearDown()
    {
        _context.Dispose();
    }
    [Test] 
    public void AddTimesheetEntries_MultipleEntriesSameDay_TotalHoursCorrect()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TimesheetDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        using var context = new TimesheetDbContext(options);


        var entry1 = TimesheetEntry.Create("John Smith", new DateTime(2024, 10, 22), "Project Alpha", "Task 1", 4);
        var entry2 = TimesheetEntry.Create("John Smith", new DateTime(2024, 10, 22), "Project Beta", "Task 2", 6);

        // Act
        context.TimesheetEntries.Add(entry1);
        context.TimesheetEntries.Add(entry2);
        context.SaveChanges();

        // Assert
        var entries = context.TimesheetEntries.ToList();

        Assert.That(entries.Count, Is.EqualTo(2)); 
        Assert.That(entries.Where(e => e.Date == new DateTime(2024, 10, 22) && e.UserName == "John Smith")
            .Sum(e => e.HoursWorked.HoursWorked), Is.EqualTo(10)); 
    }


    [Test]
    public void AddTimesheetEntry_ValidEntry_EntryAdded()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TimesheetDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        using var context = new TimesheetDbContext(options);

        var entry = TimesheetEntry.Create("testuser", DateTime.Now, "testproject", "testdescription", 5);


        // Act
        context.TimesheetEntries.Add(entry);
        context.SaveChanges();

        // Assert
        var savedEntry = context.TimesheetEntries.FirstOrDefault(x => x.UserName == entry.UserName);
        Assert.IsNotNull(savedEntry); // NUnit assertion
        Assert.That(savedEntry.UserName, Is.EqualTo(entry.UserName)); // NUnit assertion
    }
    [Test]
    public void AddTimesheetEntry_ValidData_EntryPersistedCorrectly()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TimesheetDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        using var context = new TimesheetDbContext(options);

        var userName = "John Smith";
        var date = new DateTime(2014, 10, 22);
        var project = "Project Alpha";
        var description = "Developed new feature X";
        var hoursWorked = 4;


        var entry = TimesheetEntry.Create(userName, date, project, description, hoursWorked);

        // Act
        context.TimesheetEntries.Add(entry);
        context.SaveChanges();

        // Assert
        var savedEntry = context.TimesheetEntries
            .SingleOrDefault(e => e.UserName == userName && e.Date == date); // Use SingleOrDefault for exact match


        Assert.IsNotNull(savedEntry);
        Assert.That(savedEntry.UserName, Is.EqualTo(userName));
        Assert.That(savedEntry.Date, Is.EqualTo(date));
        Assert.That(savedEntry.ProjectName, Is.EqualTo(project));
        Assert.That(savedEntry.Description, Is.EqualTo(description));
        Assert.That(savedEntry.HoursWorked.HoursWorked, Is.EqualTo(hoursWorked));

    }
    [Test]
    public void AddTimesheetEntry_HoursWorkedExceeds24_ThrowsException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TimesheetDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;
        using var context = new TimesheetDbContext(options);

        var userName = "John Smith";
        var date = new DateTime(2014, 10, 22);
        var project = "Project Alpha";
        var description = "Worked too much";
        var hoursWorked = 25;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            TimesheetEntry.Create(userName, date, project, description, hoursWorked));
    }
}