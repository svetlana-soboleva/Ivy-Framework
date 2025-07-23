import { test, expect, type Page } from '@playwright/test';

// Shared setup function for date-time input tests
async function setupDateTimeInputPage(page: Page): Promise<void> {
  await page.goto('/');
  await page.waitForLoadState('networkidle');

  // Navigate to DateTimeInput app
  const searchInput = page.getByTestId('sidebar-search');
  await expect(searchInput).toBeVisible();
  await searchInput.click();
  await searchInput.fill('date time input');
  await searchInput.press('Enter');

  const firstResult = page
    .locator('[data-sidebar="menu-item"], [data-sidebar="menu-sub-item"]')
    .filter({ hasText: /date\s*time\s*input/i })
    .first();

  await expect(firstResult).toBeVisible();
  await firstResult.click();
  await page.waitForLoadState('networkidle');
}

test.describe('DateTimeInput - Variants and Data Binding', () => {
  test.beforeEach(async ({ page }) => {
    await setupDateTimeInputPage(page);
  });

  test('should display all variants and data binding inputs', async ({
    page,
  }) => {
    // Check that the variants section exists
    await expect(
      page.locator('h2').filter({ hasText: 'Variants' })
    ).toBeVisible();

    // Check all main variant TestIds
    const mainTestIds = [
      'datetime-input-date-main',
      'datetime-input-date-disabled-main',
      'datetime-input-date-invalid-main',
      'datetime-input-date-nullable-main',
      'datetime-input-date-nullable-invalid-main',
      'datetime-input-datetime-main',
      'datetime-input-datetime-disabled-main',
      'datetime-input-datetime-invalid-main',
      'datetime-input-datetime-nullable-main',
      'datetime-input-datetime-nullable-invalid-main',
      'datetime-input-time-main',
      'datetime-input-time-disabled-main',
      'datetime-input-time-invalid-main',
      'datetime-input-time-nullable-main',
      'datetime-input-time-nullable-invalid-main',
    ];
    for (const testId of mainTestIds) {
      await expect(page.getByTestId(testId)).toBeVisible();
    }

    // Check all data binding TestIds
    const bindingTestIds = [
      'datetime-input-datetime-binding',
      'datetime-input-dateonly-binding',
      'datetime-input-timeonly-binding',
      'datetime-input-string-binding',
      'datetime-input-datetime-nullable-binding',
      'datetime-input-dateonly-nullable-binding',
      'datetime-input-timeonly-nullable-binding',
    ];
    for (const testId of bindingTestIds) {
      await expect(page.getByTestId(testId)).toBeVisible();
    }

    // Check placeholder examples
    const placeholderTestIds = [
      'datetime-input-placeholder-birthday',
      'datetime-input-placeholder-start-date',
      'datetime-input-placeholder-meeting',
      'datetime-input-placeholder-deadline',
      'datetime-input-placeholder-start-time',
      'datetime-input-placeholder-lunch-time',
    ];
    for (const testId of placeholderTestIds) {
      await expect(page.getByTestId(testId)).toBeVisible();
    }
  });

  test('should test date input interaction', async ({ page }) => {
    const dateInput = page.getByTestId('datetime-input-date-main');
    await expect(dateInput).toBeVisible();

    // Test that the input shows a date value (since it has a default value)
    await expect(dateInput).toContainText('2025-07-23');
  });

  test('should test datetime input interaction', async ({ page }) => {
    const dtInput = page.getByTestId('datetime-input-datetime-main');
    await expect(dtInput).toBeVisible();

    // Test that the input shows a date value (since it has a default value)
    await expect(dtInput).toContainText('2025-07-23');
  });

  test('should test time input interaction', async ({ page }) => {
    const timeInput = page.getByTestId('datetime-input-time-main');
    await expect(timeInput).toBeVisible();

    // For time inputs, we should test that the time input element exists
    const timeInputElement = timeInput.locator('input[type="time"]');
    await expect(timeInputElement).toBeVisible();
  });

  test('should test nullable date input', async ({ page }) => {
    const nullableInput = page.getByTestId('datetime-input-date-nullable-main');
    await expect(nullableInput).toBeVisible();

    // Test that the input shows placeholder for nullable state
    await expect(nullableInput).toContainText('Pick a date');
  });

  test('should test disabled date input', async ({ page }) => {
    const disabledInput = page.getByTestId('datetime-input-date-disabled-main');
    await expect(disabledInput).toBeVisible();

    // Test that the input is disabled
    await expect(disabledInput).toBeDisabled();
  });

  test('should test data binding inputs', async ({ page }) => {
    // Test that data binding inputs are visible and interactive
    const bindingInputs = [
      'datetime-input-datetime-binding',
      'datetime-input-dateonly-binding',
      'datetime-input-timeonly-binding',
      'datetime-input-string-binding',
    ];

    for (const testId of bindingInputs) {
      const input = page.getByTestId(testId);
      await expect(input).toBeVisible();
    }
  });

  test('should test placeholder examples', async ({ page }) => {
    // Test that placeholder examples show the correct placeholder text
    const birthdayInput = page.getByTestId(
      'datetime-input-placeholder-birthday'
    );
    await expect(birthdayInput).toContainText('Birthday');

    const meetingInput = page.getByTestId('datetime-input-placeholder-meeting');
    await expect(meetingInput).toContainText('Meeting time');

    // For time inputs, test that the element exists but don't check for placeholder text
    const startTimeInput = page.getByTestId(
      'datetime-input-placeholder-start-time'
    );
    await expect(startTimeInput).toBeVisible();
  });
});
