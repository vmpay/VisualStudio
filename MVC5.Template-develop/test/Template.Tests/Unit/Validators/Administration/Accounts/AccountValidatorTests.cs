﻿using Template.Components.Alerts;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.Administration.Accounts.AccountView;
using Template.Tests.Data;
using Template.Validators;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace Template.Tests.Unit.Validators
{
    public class AccountValidatorTests : IDisposable
    {
        private AccountValidator validator;
        private TestingContext context;
        private Account account;
        private IHasher hasher;

        public AccountValidatorTests()
        {
            context = new TestingContext();
            hasher = Substitute.For<IHasher>();
            hasher.VerifyPassword(Arg.Any<String>(), Arg.Any<String>()).Returns(true);

            context.DropData();
            SetUpData();

            validator = new AccountValidator(new UnitOfWork(context), hasher);
            validator.CurrentAccountId = account.Id;
        }
        public void Dispose()
        {
            validator.Dispose();
            context.Dispose();
        }

        #region Method: CanRegister(AccountRegisterView view)

        [Fact]
        public void CanRegister_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanRegister(ObjectFactory.CreateAccountRegisterView()));
        }

        [Fact]
        public void CanRegister_UsedUsername_ReturnsFalse()
        {
            AccountRegisterView view = ObjectFactory.CreateAccountRegisterView(2);
            view.Username = account.Username.ToLower();

            Boolean canRegister = validator.CanRegister(view);

            Assert.False(canRegister);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.UsernameIsAlreadyTaken, validator.ModelState["Username"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanRegister_UsedEmail_ReturnsFalse()
        {
            AccountRegisterView view = ObjectFactory.CreateAccountRegisterView(2);
            view.Email = account.Email;

            Boolean canRegister = validator.CanRegister(view);

            Assert.False(canRegister);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.EmailIsAlreadyUsed, validator.ModelState["Email"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanRegister_ValidAccount()
        {
            Assert.True(validator.CanRegister(ObjectFactory.CreateAccountRegisterView(2)));
        }

        #endregion

        #region Method: CanRecover(AccountRecoveryView view)

        [Fact]
        public void CanRecover_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        [Fact]
        public void CanRecover_ValidAccount()
        {
            Assert.True(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        #endregion

        #region Method: CanReset(AccountResetView view)

        [Fact]
        public void CanReset_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanReset(ObjectFactory.CreateAccountResetView()));
        }

        [Fact]
        public void CanReset_ExpiredToken_ReturnsFalse()
        {
            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(-5);
            context.SaveChanges();

            Boolean canReset = validator.CanReset(ObjectFactory.CreateAccountResetView());
            Alert alert = validator.Alerts.Single();

            Assert.False(canReset);
            Assert.Empty(validator.ModelState);
            Assert.Equal(0, alert.FadeoutAfter);
            Assert.Equal(AlertType.Danger, alert.Type);
            Assert.Equal(Validations.RecoveryTokenExpired, alert.Message);
        }

        [Fact]
        public void CanReset_ValidAccount()
        {
            Assert.True(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        #endregion

        #region Method: CanLogin(AccountLoginView view)

        [Fact]
        public void CanLogin_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanLogin(ObjectFactory.CreateAccountLoginView()));
        }

        [Fact]
        public void CanLogin_NoAccount_ReturnsFalse()
        {
            hasher.VerifyPassword(null, null).Returns(false);
            AccountLoginView view = new AccountLoginView();

            Boolean canLogin = validator.CanLogin(view);

            Assert.False(canLogin);
            Assert.Empty(validator.Alerts);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.IncorrectAuthentication, validator.ModelState[""].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanLogin_IncorrectPassword_ReturnsFalse()
        {
            account = context.Set<Account>().Single();
            account.IsLocked = true;
            context.SaveChanges();

            AccountLoginView view = ObjectFactory.CreateAccountLoginView();
            hasher.VerifyPassword(view.Password, Arg.Any<String>()).Returns(false);

            Boolean canLogin = validator.CanLogin(view);

            Assert.False(canLogin);
            Assert.Empty(validator.Alerts);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.IncorrectAuthentication, validator.ModelState[""].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanLogin_LockedAccount_ReturnsFalse()
        {
            AccountLoginView view = ObjectFactory.CreateAccountLoginView();
            account = context.Set<Account>().Single();
            account.IsLocked = true;
            context.SaveChanges();

            Boolean canLogin = validator.CanLogin(view);
            Alert alert = validator.Alerts.Single();

            Assert.False(canLogin);
            Assert.Empty(validator.ModelState);
            Assert.Equal(0, alert.FadeoutAfter);
            Assert.Equal(AlertType.Danger, alert.Type);
            Assert.Equal(Validations.AccountIsLocked, alert.Message);
        }

        [Fact]
        public void CanLogin_IsCaseInsensitive()
        {
            AccountLoginView view = ObjectFactory.CreateAccountLoginView();
            view.Username = view.Username.ToUpper();

            Assert.True(validator.CanLogin(view));
        }

        [Fact]
        public void CanLogin_ValidAccount()
        {
            Assert.True(validator.CanLogin(ObjectFactory.CreateAccountLoginView()));
        }

        #endregion

        #region Method: CanCreate(AccountCreateView view)

        [Fact]
        public void CanCreate_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanCreate(ObjectFactory.CreateAccountCreateView()));
        }

        [Fact]
        public void CanCreate_UsedUsername_ReturnsFalse()
        {
            AccountCreateView view = ObjectFactory.CreateAccountCreateView(2);
            view.Username = account.Username.ToLower();

            Boolean canCreate = validator.CanCreate(view);

            Assert.False(canCreate);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.UsernameIsAlreadyTaken, validator.ModelState["Username"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanCreate_UsedEmail_ReturnsFalse()
        {
            AccountCreateView view = ObjectFactory.CreateAccountCreateView(2);
            view.Email = account.Email.ToUpper();

            Boolean canCreate = validator.CanCreate(view);

            Assert.False(canCreate);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.EmailIsAlreadyUsed, validator.ModelState["Email"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanCreate_ValidAccount()
        {
            Assert.True(validator.CanCreate(ObjectFactory.CreateAccountCreateView(2)));
        }

        #endregion

        #region Method: CanEdit(AccountEditView view)

        [Fact]
        public void CanEdit_InvalidAccountState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanEdit(ObjectFactory.CreateAccountEditView()));
        }

        [Fact]
        public void CanEdit_ValidAccount()
        {
            Assert.True(validator.CanEdit(ObjectFactory.CreateAccountEditView()));
        }

        #endregion

        #region Method: CanEdit(ProfileEditView view)

        [Fact]
        public void CanEdit_InvalidProfileState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanEdit(ObjectFactory.CreateProfileEditView()));
        }

        [Fact]
        public void CanEdit_IncorrectPassword_ReturnsFalse()
        {
            ProfileEditView view = ObjectFactory.CreateProfileEditView(1577);
            hasher.VerifyPassword(view.Password, Arg.Any<String>()).Returns(false);

            Boolean canEdit = validator.CanEdit(view);

            Assert.False(canEdit);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.IncorrectPassword, validator.ModelState["Password"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanEdit_UsedUsername_ReturnsFalse()
        {
            Account takenAccount = ObjectFactory.CreateAccount(2);
            context.Set<Account>().Add(takenAccount);
            context.SaveChanges();

            ProfileEditView view = ObjectFactory.CreateProfileEditView();
            view.Username = takenAccount.Username.ToLower();

            Boolean canEdit = validator.CanEdit(view);

            Assert.False(canEdit);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.UsernameIsAlreadyTaken, validator.ModelState["Username"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanEdit_ToSameUsername()
        {
            ProfileEditView view = ObjectFactory.CreateProfileEditView(2);
            view.Username = account.Username.ToUpper();

            Assert.True(validator.CanEdit(view));
        }

        [Fact]
        public void CanEdit_UsedEmail_ReturnsFalse()
        {
            Account usedEmailAccount = ObjectFactory.CreateAccount(2);
            context.Set<Account>().Add(usedEmailAccount);
            context.SaveChanges();

            ProfileEditView view = ObjectFactory.CreateProfileEditView();
            view.Email = usedEmailAccount.Email;

            Boolean canEdit = validator.CanEdit(view);

            Assert.False(canEdit);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.EmailIsAlreadyUsed, validator.ModelState["Email"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanEdit_ToSameEmail()
        {
            ProfileEditView view = ObjectFactory.CreateProfileEditView(2);
            view.Email = account.Email.ToUpper();

            Assert.True(validator.CanEdit(view));
        }

        [Fact]
        public void CanEdit_ValidProfile()
        {
            Assert.True(validator.CanEdit(ObjectFactory.CreateProfileEditView(1457)));
        }

        #endregion

        #region Method: CanDelete(ProfileDeleteView view)

        [Fact]
        public void CanDelete_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanDelete(ObjectFactory.CreateProfileDeleteView()));
        }

        [Fact]
        public void CanDelete_IncorrectPassword_ReturnsFalse()
        {
            ProfileDeleteView view = ObjectFactory.CreateProfileDeleteView();
            hasher.VerifyPassword(view.Password, Arg.Any<String>()).Returns(false);

            Boolean canDelete = validator.CanDelete(view);

            Assert.False(canDelete);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.IncorrectPassword, validator.ModelState["Password"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanDelete_ValidProfile()
        {
            Assert.True(validator.CanDelete(ObjectFactory.CreateProfileDeleteView()));
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            account = ObjectFactory.CreateAccount();
            account.Role = ObjectFactory.CreateRole();
            account.RoleId = account.Role.Id;
            account.IsLocked = false;

            context.Set<Account>().Add(account);
            context.SaveChanges();
        }

        #endregion
    }
}
