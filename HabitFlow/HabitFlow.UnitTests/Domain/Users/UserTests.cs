using FluentAssertions;
using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Users;
using HabitFlow.Domain.Users.Events;

namespace HabitFlow.UnitTests.Domain.Users
{
    [TestFixture]
    public class UserTests
    {
        private const string ValidEmail = "joao@exemplo.com";
        private const string ValidHash = "hash_seguro_qualquer";
        private const string ValidFirstName = "Joao";
        private const string ValidLastName = "Silva";

        private static User CreateValidUser(
            string email = ValidEmail,
            string hash = ValidHash,
            string firstName = ValidFirstName,
            string lastName = ValidLastName) =>
            User.Create(email, hash, firstName, lastName);

        // ── Create ────────────────────────────────────────────────────────────────

        [Test]
        public void Create_ValidInputs_ReturnsUser()
        {
            var user = CreateValidUser();

            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Email.Should().Be(ValidEmail);
            user.PasswordHash.Should().Be(ValidHash);
            user.FirstName.Should().Be(ValidFirstName);
            user.LastName.Should().Be(ValidLastName);
            user.IsActive.Should().BeTrue();
            user.IsEmailVerified.Should().BeFalse();
        }

        [Test]
        public void Create_NormalizesEmailToLowercase()
        {
            var user = User.Create("JOAO@EXEMPLO.COM", ValidHash, ValidFirstName, ValidLastName);

            user.Email.Should().Be("joao@exemplo.com");
        }

        [Test]
        public void Create_WithoutDisplayName_DefaultsToFullName()
        {
            var user = CreateValidUser();

            user.DisplayName.Should().Be("Joao Silva");
        }

        [Test]
        public void Create_WithExplicitDisplayName_UsesProvided()
        {
            var user = User.Create(ValidEmail, ValidHash, ValidFirstName, ValidLastName, "JoaoS");

            user.DisplayName.Should().Be("JoaoS");
        }

        [Test]
        public void Create_TrimsNames()
        {
            var user = User.Create(ValidEmail, ValidHash, "  Joao  ", "  Silva  ");

            user.FirstName.Should().Be("Joao");
            user.LastName.Should().Be("Silva");
        }

        [Test]
        public void Create_RaisesUserRegisteredEvent()
        {
            var user = CreateValidUser();

            user.DomainEvents.Should().ContainSingle(e => e is UserRegisteredEvent);
            var evt = (UserRegisteredEvent)user.DomainEvents.Single();
            evt.UserId.Should().Be(user.Id);
            evt.Email.Should().Be(ValidEmail);
        }

        [Test]
        public void Create_SetsDefaultProfileAndSettings()
        {
            var user = CreateValidUser();

            user.Profile.Should().NotBeNull();
            user.Settings.Should().NotBeNull();
        }

        // ── Validation ────────────────────────────────────────────────────────────

        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyEmail_ThrowsValidationException(string email)
        {
            var act = () => User.Create(email, ValidHash, ValidFirstName, ValidLastName);

            act.Should().Throw<ValidationException>()
                .WithMessage("Email is required");
        }

        [Test]
        public void Create_EmailExceedsMaxLength_ThrowsValidationException()
        {
            var longEmail = new string('a', 246) + "@b.com";
            var act = () => User.Create(longEmail, ValidHash, ValidFirstName, ValidLastName);

            act.Should().Throw<ValidationException>();
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyPasswordHash_ThrowsValidationException(string hash)
        {
            var act = () => User.Create(ValidEmail, hash, ValidFirstName, ValidLastName);

            act.Should().Throw<ValidationException>()
                .WithMessage("Password hash is required");
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyFirstName_ThrowsValidationException(string firstName)
        {
            var act = () => User.Create(ValidEmail, ValidHash, firstName, ValidLastName);

            act.Should().Throw<ValidationException>()
                .WithMessage("First name is required");
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyLastName_ThrowsValidationException(string lastName)
        {
            var act = () => User.Create(ValidEmail, ValidHash, ValidFirstName, lastName);

            act.Should().Throw<ValidationException>()
                .WithMessage("Last name is required");
        }

        // ── UpdateProfile ─────────────────────────────────────────────────────────

        [Test]
        public void UpdateProfile_ValidInputs_UpdatesFields()
        {
            var user = CreateValidUser();
            user.ClearDomainEvents();

            user.UpdateProfile("Maria", "Santos", "MariaSantos");

            user.FirstName.Should().Be("Maria");
            user.LastName.Should().Be("Santos");
            user.DisplayName.Should().Be("MariaSantos");
        }

        [Test]
        public void UpdateProfile_RaisesUserProfileUpdatedEvent()
        {
            var user = CreateValidUser();
            user.ClearDomainEvents();

            user.UpdateProfile("Maria", "Santos", "MariaSantos");

            user.DomainEvents.Should().ContainSingle(e => e is UserProfileUpdatedEvent);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void UpdateProfile_EmptyFirstName_ThrowsValidationException(string firstName)
        {
            var user = CreateValidUser();

            var act = () => user.UpdateProfile(firstName, "Santos", "MS");

            act.Should().Throw<ValidationException>()
                .WithMessage("First name is required");
        }

        // ── UpdatePassword ────────────────────────────────────────────────────────

        [Test]
        public void UpdatePassword_ValidHash_UpdatesPasswordHash()
        {
            var user = CreateValidUser();

            user.UpdatePassword("novo_hash_seguro");

            user.PasswordHash.Should().Be("novo_hash_seguro");
        }

        [TestCase("")]
        [TestCase("   ")]
        public void UpdatePassword_EmptyHash_ThrowsValidationException(string hash)
        {
            var user = CreateValidUser();

            var act = () => user.UpdatePassword(hash);

            act.Should().Throw<ValidationException>();
        }

        // ── VerifyEmail ───────────────────────────────────────────────────────────

        [Test]
        public void VerifyEmail_SetsIsEmailVerifiedTrue()
        {
            var user = CreateValidUser();

            user.VerifyEmail();

            user.IsEmailVerified.Should().BeTrue();
        }

        // ── AggregateRoot behavior ────────────────────────────────────────────────

        [Test]
        public void ClearDomainEvents_RemovesAllEvents()
        {
            var user = CreateValidUser();
            user.UpdateProfile("Maria", "Santos", "MS");

            user.ClearDomainEvents();

            user.DomainEvents.Should().BeEmpty();
        }

        [Test]
        public void TwoUsers_HaveDifferentIds()
        {
            var user1 = CreateValidUser();
            var user2 = User.Create("outro@email.com", ValidHash, ValidFirstName, ValidLastName);

            user1.Id.Should().NotBe(user2.Id);
        }
    }
}
