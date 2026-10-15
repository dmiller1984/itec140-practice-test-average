# Test Average

This is an original, unmarked practice exercise. It is not handed in and does not predict Richard's quiz questions.

**Purpose:** Enter three test scores. Calculate their average and display whether it reaches the pass mark of **60%**.

Each test has equal weight. **Add the three scores together, then divide the total by 3.**

Optional practice time: **50 minutes**.

## Before you begin

- Open **TestAveragePractice.sln** in Visual Studio 2026 with .NET 10 and Windows Forms support. Work on **Form1** in **TestAveragePractice**.
- Use the exact names and message spelling below.
- Enter numbers such as **75** or **82.5**, without a percent sign. Assume numeric scores are between **0 and 100**; range checks are not required.
- Make at least **one Git commit for each numbered section** below. You may make more.

## 1. Create four fields of type double [4 marks]

Use these exact field names:

- dblScoreOne
- dblScoreTwo
- dblScoreThree
- dblAverage

## 2. Create the following controls [8 marks]

| Name | Type | Purpose | Enabled |
| --- | --- | --- | --- |
| txtScoreOne | TextBox | First test score | True |
| txtScoreTwo | TextBox | Second test score | True |
| txtScoreThree | TextBox | Third test score | True |
| txtAverage | TextBox | Calculated average | **False** |
| btnCalculate | Button | Calculates the result | True |
| picScores | PictureBox | Supplied image | True |
| lblNote | Label | Pass result | True |
| lblError | Label | Invalid input message | True |

Change the **Enabled** property of **txtAverage** to **False**.

The image is already on the form in **pictureBox1**. Change that PictureBox's **(Name)** to **picScores**. Only the control needs renaming.

Add labels to identify each text box. These extra labels and your layout are not marked. A separate label may show **%** beside the average.

## 3. Calculate and display the result [8 marks]

When **btnCalculate** is clicked:

- Set **dblScoreOne** to the number parsed from **txtScoreOne**.
- Set **dblScoreTwo** to the number parsed from **txtScoreTwo**.
- Set **dblScoreThree** to the number parsed from **txtScoreThree**.
- Set **dblAverage** to the average of the three scores: their total divided by **3**.
- Make one decision: if **dblAverage** is **60 or higher**, set **lblNote** to exactly **Pass**. Otherwise, set it to exactly **Below pass mark**.
- Display **dblAverage** in **txtAverage** using **.ToString("N2")**.

Show **two decimal places** in **txtAverage**, without a percent sign or dollar sign. Use the calculated average for the pass decision.

The average output is worth **4 marks**. The text in **lblNote** is worth **4 marks**.

## 4. Handle invalid input [3 marks]

If **any** score box contains a nonnumeric value or is blank, handle the error and display this exact text in **lblError**:

**Please enter a proper number**

For example, **2.0A** is invalid. **82.5** is valid.

Clearing an earlier error after a successful calculation is optional and not marked.

## Check your results

| First score | Second score | Third score | txtAverage | lblNote |
| --- | --- | --- | --- | --- |
| 80 | 70 | 90 | 80.00 | Pass |
| 60 | 60 | 60 | 60.00 | Pass |
| 40 | 50 | 60 | 50.00 | Below pass mark |
| 72 | 73 | 74 | 73.00 | Pass |

Also test invalid input in each score box, one at a time.

## Run the practice checks

**TestProject1** contains **23 tests**, one per mark.

1. Build the solution.
2. In Visual Studio, open **Test → Test Explorer**.
3. Click **Run All Tests**.
4. Read failed test messages to see which requirements need work.

The starting project is expected to fail the checks. Complete the exercise, then run them again.

## Marks

| Section | Marks |
| --- | --- |
| Four fields | 4 |
| Eight controls | 8 |
| Output in txtAverage | 4 |
| Text in lblNote | 4 |
| Error handling | 3 |
| **Total** | **23** |

## Practice expectations

- Try it on your own first. Use your notes or look things up if needed.
- Be able to explain each part of your work.
- Make the four section commits.

## References

Microsoft references for the beginner skills used here:

- [Add decision logic to your code using if, else, and else if statements in C#](https://learn.microsoft.com/en-us/training/modules/csharp-if-elseif-else/)
- [Create a Windows Forms app with Visual Studio](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/get-started/create-app-visual-studio)
