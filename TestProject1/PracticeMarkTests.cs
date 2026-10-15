using System.Globalization;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAveragePractice;

[assembly: DoNotParallelize]

namespace TestProject1;

// Each test is worth one practice mark. The unfinished starter is expected to fail.
// Reflection lets these tests build before the required fields and controls exist.
[TestClass]
public class PracticeMarkTests
{
    [TestMethod]
    [TestCategory("Fields")]
    public void Mark01_DblScoreOne_IsADoubleField() => AssertField("dblScoreOne", typeof(double));

    [TestMethod]
    [TestCategory("Fields")]
    public void Mark02_DblScoreTwo_IsADoubleField() => AssertField("dblScoreTwo", typeof(double));

    [TestMethod]
    [TestCategory("Fields")]
    public void Mark03_DblScoreThree_IsADoubleField() => AssertField("dblScoreThree", typeof(double));

    [TestMethod]
    [TestCategory("Fields")]
    public void Mark04_DblAverage_IsADoubleField() => AssertField("dblAverage", typeof(double));

    [TestMethod]
    [TestCategory("Controls")]
    public void Mark05_TxtScoreOne_IsATextBox() => AssertControl<TextBox>("txtScoreOne");

    [TestMethod]
    [TestCategory("Controls")]
    public void Mark06_TxtScoreTwo_IsATextBox() => AssertControl<TextBox>("txtScoreTwo");

    [TestMethod]
    [TestCategory("Controls")]
    public void Mark07_TxtScoreThree_IsATextBox() => AssertControl<TextBox>("txtScoreThree");

    [TestMethod]
    [TestCategory("Controls")]
    public void Mark08_TxtAverage_IsADisabledTextBox()
    {
        OnForm(form =>
        {
            TextBox output = RequiredControl<TextBox>(form, "txtAverage");
            Assert.IsFalse(output.Enabled, "Set txtAverage.Enabled to False in the Designer.");
        });
    }

    [TestMethod]
    [TestCategory("Controls")]
    public void Mark09_BtnCalculate_IsAButton() => AssertControl<Button>("btnCalculate");

    [TestMethod]
    [TestCategory("Controls")]
    public void Mark10_PicScores_IsAPictureBox() => AssertControl<PictureBox>("picScores");

    [TestMethod]
    [TestCategory("Controls")]
    public void Mark11_LblNote_IsALabel() => AssertControl<Label>("lblNote");

    [TestMethod]
    [TestCategory("Controls")]
    public void Mark12_LblError_IsALabel() => AssertControl<Label>("lblError");

    [TestMethod]
    [TestCategory("Average output")]
    public void Mark13_BelowPassMark_DisplaysTheAverageOfAllThreeScores()
    {
        OnForm(form =>
        {
            Calculate(form, "20", "50", "65");
            AssertAverage(form, 45.0, "45.00");
        });
    }

    [TestMethod]
    [TestCategory("Average output")]
    public void Mark14_ExactlyAtPassMark_DisplaysTheAverage()
    {
        OnForm(form =>
        {
            Calculate(form, "45", "60", "75");
            AssertAverage(form, 60.0, "60.00");
        });
    }

    [TestMethod]
    [TestCategory("Average output")]
    public void Mark15_AbovePassMark_DisplaysTheAverage()
    {
        OnForm(form =>
        {
            Calculate(form, "60", "90", "99");
            AssertAverage(form, 83.0, "83.00");
        });
    }

    [TestMethod]
    [TestCategory("Average output")]
    public void Mark16_FractionalScores_DisplaysTheAverageRoundedToTwoDecimalPlaces()
    {
        OnForm(form =>
        {
            Calculate(form, "59.5", "60.25", "61.5");
            AssertAverage(form, 60.416666666666664, "60.42");
        });
    }

    [TestMethod]
    [TestCategory("Note output")]
    public void Mark17_JustBelowPassMark_ShowsBelowPassMark()
    {
        OnForm(form =>
        {
            Calculate(form, "59.99", "59.99", "59.99");
            AssertNote(form, "Below pass mark");
        });
    }

    [TestMethod]
    [TestCategory("Note output")]
    public void Mark18_ExactlyAtPassMark_ShowsPass()
    {
        OnForm(form =>
        {
            Calculate(form, "45", "60", "75");
            AssertNote(form, "Pass");
        });
    }

    [TestMethod]
    [TestCategory("Note output")]
    public void Mark19_AbovePassMark_ShowsPass()
    {
        OnForm(form =>
        {
            Calculate(form, "72", "88", "95");
            AssertNote(form, "Pass");
        });
    }

    [TestMethod]
    [TestCategory("Note output")]
    public void Mark20_CalculatingAgain_UpdatesTheAverageAndNote()
    {
        OnForm(form =>
        {
            Calculate(form, "20", "50", "65");
            AssertNote(form, "Below pass mark");
            AssertAverage(form, 45.0, "45.00");
            Calculate(form, "60", "90", "99");
            AssertNote(form, "Pass");
            AssertAverage(form, 83.0, "83.00");
            Calculate(form, "30", "50", "70");
            AssertNote(form, "Below pass mark");
            AssertAverage(form, 50.0, "50.00");
        });
    }

    [TestMethod]
    [TestCategory("Error handling")]
    public void Mark21_InvalidScoreOne_ShowsTheExactErrorMessage()
    {
        AssertInvalidInput("2.0A", "60", "75");
        AssertInvalidInput("", "60", "75");
    }

    [TestMethod]
    [TestCategory("Error handling")]
    public void Mark22_InvalidScoreTwo_ShowsTheExactErrorMessage()
    {
        AssertInvalidInput("45", "2.0A", "75");
        AssertInvalidInput("45", "", "75");
    }

    [TestMethod]
    [TestCategory("Error handling")]
    public void Mark23_InvalidScoreThree_ShowsTheExactErrorMessage()
    {
        AssertInvalidInput("45", "60", "2.0A");
        AssertInvalidInput("45", "60", "");
    }

    private static void AssertField(string name, Type expectedType)
    {
        FieldInfo? field = typeof(Form1).GetField(name,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        Assert.IsNotNull(field,
            $"Declare {name} inside the Form1 class and outside every method. Use the exact spelling.");
        Assert.AreEqual(expectedType, field!.FieldType, $"{name} must have type {expectedType.Name}.");
    }

    private static void AssertControl<T>(string name) where T : Control
        => OnForm(form => { _ = RequiredControl<T>(form, name); });

    private static T RequiredControl<T>(Form form, string name) where T : Control
    {
        List<Control> matches = Descendants(form).Where(control => control.Name == name).ToList();
        Assert.AreEqual(1, matches.Count,
            $"Expected exactly one control named {name}. Check its (Name) property, including capitals.");
        Assert.IsInstanceOfType(matches[0], typeof(T), $"{name} must be a {typeof(T).Name}.");
        return (T)matches[0];
    }

    private static IEnumerable<Control> Descendants(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            yield return child;
            foreach (Control descendant in Descendants(child))
            {
                yield return descendant;
            }
        }
    }

    private static void Calculate(Form form, string scoreOne, string scoreTwo, string scoreThree)
    {
        RequiredControl<TextBox>(form, "txtScoreOne").Text = scoreOne;
        RequiredControl<TextBox>(form, "txtScoreTwo").Text = scoreTwo;
        RequiredControl<TextBox>(form, "txtScoreThree").Text = scoreThree;
        Button button = RequiredControl<Button>(form, "btnCalculate");
        Assert.IsTrue(button.Enabled && button.Visible, "btnCalculate must be enabled and visible.");
        button.PerformClick();
    }

    private static void AssertAverage(Form form, double expectedValue, string expectedDisplay)
    {
        FieldInfo? field = typeof(Form1).GetField("dblAverage",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        Assert.IsNotNull(field, "Declare dblAverage as a double field on Form1.");
        Assert.AreEqual(typeof(double), field!.FieldType, "dblAverage must be a double field.");
        Assert.AreEqual(expectedValue, (double)field.GetValue(form)!, 0.000000001,
            "dblAverage must contain the average of all three scores.");
        Assert.AreEqual(expectedDisplay, RequiredControl<TextBox>(form, "txtAverage").Text,
            "Display the average with N2 formatting: exactly two decimal places, without a currency or percent sign.");
    }

    private static void AssertNote(Form form, string expectedText)
        => Assert.AreEqual(expectedText, RequiredControl<Label>(form, "lblNote").Text,
            "lblNote must contain the exact text in the README, with no extra punctuation or spaces.");

    private static void AssertInvalidInput(string scoreOne, string scoreTwo, string scoreThree)
    {
        OnForm(form =>
        {
            Calculate(form, scoreOne, scoreTwo, scoreThree);
            Assert.AreEqual("Please enter a proper number", RequiredControl<Label>(form, "lblError").Text,
                "Handle bad input and put the exact required message in lblError.");
        });
    }

    private static void OnForm(Action<Form1> check)
    {
        Exception? failure = null;
        Thread uiThread = new(() =>
        {
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-CA");
                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-CA");
                using Form1 form = new();
                form.ShowInTaskbar = false;
                form.Show();
                Application.DoEvents();
                check(form);
            }
            catch (Exception exception)
            {
                failure = exception;
            }
        })
        {
            IsBackground = true
        };

        uiThread.SetApartmentState(ApartmentState.STA);
        uiThread.Start();
        Assert.IsTrue(uiThread.Join(TimeSpan.FromSeconds(15)),
            "The form did not finish this check within 15 seconds. Close any modal dialog and check the button code.");
        if (failure is not null)
        {
            ExceptionDispatchInfo.Capture(failure).Throw();
        }
    }
}
