using LogSummitApi.Application.Core.Utilities;

namespace LogSummitApi.Tests.Unit;

public class HasherTests
{
    [Fact]
    public void Hash_WorksAndReturnsHashedText()
    {
        // prepare
        var hasher = new Hasher();
        var plainText = "password123";

        // act & assert
        var hashedText = hasher.Hash(plainText);

        hashedText.Should().NotBe(plainText);
        hashedText.Length.Should().BeGreaterThan(plainText.Length);
    }

    [Fact]
    public void Compare_WorksAndReturnsTrueOrFalse()
    {
        // prepare
        var hasher = new Hasher();
        var plainText = "password123";
        var hashedText = hasher.Hash(plainText);

        // act & assert
        hasher.Compare(plainText, hashedText).Should().BeTrue();
        hasher.Compare("wrong_password", hashedText).Should().BeFalse();
    }

    [Fact]
    public void Compare_ValidationWorks()
    {
        // prepare
        var hasher = new Hasher();

        // act & assert
        Assert.Throws<FormatException>(() => hasher.Compare("password123", "invalid_hash"));
        Assert.Throws<FormatException>(() => hasher.Compare("wrong_password", "asdas;sada"));
    }
}
