using CommitLint.Net.Models;
using CommitLint.Net.Validators;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.ValidatorsTests;

[TestFixture]
public class AdditionalRulesValidatorTests
{
    [Test]
    public void WhenSubjectExceedsMaxLength_ThenReturnInvalidResult()
    {
        // Arrange
        var config = GetConfig(maxSubjectLength: 10);
        var subject = new AdditionalRulesValidator(config);
        string[] commitMessageLines = ["feat: description"];

        // Act
        var result = subject.Validate(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    private static CommitMessageConfig GetConfig(int? maxSubjectLength = null)
    {
        return new CommitMessageConfig
        {
            MaxSubjectLength = new MaxSubjectLength
            {
                Enabled = true,
                Value = maxSubjectLength ?? 50,
            },
        };
    }
}
