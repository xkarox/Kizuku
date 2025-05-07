using Backend.Validators;
using Core;
using Core.Errors.Authentication;

namespace BackendTest.Unit.Validators;

[TestFixture]
[Parallelizable]
public class PasswordValidatorTests
{
    PasswordValidator _sut;
    
    [SetUp]
    public void Setup()
    {
        _sut = new PasswordValidator();
    }
    
    /// <summary>
    /// Validates that the password validator returns success for a password that:
    /// - Has a minimum length of 8 characters.
    /// - Contains at least one uppercase letter.
    /// - Contains at least one lowercase letter.
    /// - Contains at least one digit.
    /// - Contains at least one special character.
    /// </summary>
    [Test]
    public async Task Validate_WithValidPassword_ShouldReturnTrue()
    {
        const string validPassword = "Aa0@____";
        
        var validationResult = _sut.Validate(validPassword);
        
        Assert.That(validationResult.IsSuccess, Is.True);
    }
    
    [Test]
    public async Task Validate_WithPasswordLengthSeven_ShouldReturnFalse()
    {
        const string shortPassword = "Aa0@___";
        
        var validationResult = _sut.Validate(shortPassword);
        
        Assert.That(validationResult.IsError, Is.True);
        var error = validationResult.Error;
        Assert.That(error, Is.TypeOf<PasswordRegistrationValidationError>());
        Assert.That((error as PasswordRegistrationValidationError)!.HasMinLength, Is.False);
        Assert.That((error as PasswordRegistrationValidationError)!.HasUpperCase, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasLowerCase, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasDigit, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasSpecialChar, Is.True);
    }
    
    [Test]
    public async Task Validate_WithPasswordWithoutUppercaseCharacter_ShouldReturnFalse()
    {
        const string shortPassword = "aa0@____";
        
        var validationResult = _sut.Validate(shortPassword);
        
        Assert.That(validationResult.IsError, Is.True);
        var error = validationResult.Error;
        Assert.That(error, Is.TypeOf<PasswordRegistrationValidationError>());
        Assert.That((error as PasswordRegistrationValidationError)!.HasMinLength, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasUpperCase, Is.False);
        Assert.That((error as PasswordRegistrationValidationError)!.HasLowerCase, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasDigit, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasSpecialChar, Is.True);
    }
    
    [Test]
    public async Task Validate_WithPasswordWithoutLowecaseCharacter_ShouldReturnFalse()
    {
        const string shortPassword = "AA0@____";
        
        var validationResult = _sut.Validate(shortPassword);
        
        Assert.That(validationResult.IsError, Is.True);
        var error = validationResult.Error;
        Assert.That(error, Is.TypeOf<PasswordRegistrationValidationError>());
        Assert.That((error as PasswordRegistrationValidationError)!.HasMinLength, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasUpperCase, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasLowerCase, Is.False);
        Assert.That((error as PasswordRegistrationValidationError)!.HasDigit, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasSpecialChar, Is.True);
    }
    
    [Test]
    public async Task Validate_WithPasswordWithoutDigit_ShouldReturnFalse()
    {
        const string shortPassword = "Aaa@____";
        
        var validationResult = _sut.Validate(shortPassword);
        
        Assert.That(validationResult.IsError, Is.True);
        var error = validationResult.Error;
        Assert.That(error, Is.TypeOf<PasswordRegistrationValidationError>());
        Assert.That((error as PasswordRegistrationValidationError)!.HasMinLength, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasUpperCase, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasLowerCase, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasDigit, Is.False);
        Assert.That((error as PasswordRegistrationValidationError)!.HasSpecialChar, Is.True);
    }
    
    [Test]
    public async Task Validate_Validate_WithPasswordWithoutSpecialCharacter_ShouldReturnFalse()
    {
        const string shortPassword = "Aa00aaaa";
        
        var validationResult = _sut.Validate(shortPassword);
        
        Assert.That(validationResult.IsError, Is.True);
        var error = validationResult.Error;
        Assert.That(error, Is.TypeOf<PasswordRegistrationValidationError>());
        Assert.That((error as PasswordRegistrationValidationError)!.HasMinLength, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasUpperCase, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasLowerCase, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasDigit, Is.True);
        Assert.That((error as PasswordRegistrationValidationError)!.HasSpecialChar, Is.False);
    }
}