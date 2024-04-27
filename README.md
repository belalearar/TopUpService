User Story:
As a user of the Edenred mobile application, I want to top up my UAE phone numbers with
credit so I can make local phone calls.

Acceptance Criteria:
• The user can add a maximum of 5 active top-up beneficiaries.
• Each top-up beneficiary must have a nickname with a maximum length of 20
characters.
• The user should be able to view available top-up beneficiaries.
• The user should be able to view available top-up options (AED 5, AED 10, AED 20, AED 30,
AED 50, AED 75, AED 100).
• If a user is not verified, they can top up a maximum of AED 1,000 per calendar month per
beneficiary for security reasons.
• If a user is verified, they can top up a maximum of AED 500 per calendar month per
beneficiary.
• The user can top up multiple beneficiaries but is limited to a total of AED 3,000 per month
for all beneficiaries.
• A charge of AED 1 should be applied for every top-up transaction.
• The user can only top up with an amount equal to or less than their balance which will be
retrieved from an external HTTP service.
• The user's balance should be debited first before the top-up transaction is attempted.

Additional Notes:
• The user's verification status is assumed to be handled through a separate feature that is
not within the scope of this user story, you can assume the verification flag is one of the
properties of the user entity.
• The external HTTP service will be responsible for providing real-time balance information
and debit/ credit functionality, we expect you to build this HTTP service and consume it
within the top up feature.
