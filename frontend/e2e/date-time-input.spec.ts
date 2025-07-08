import { test, expect } from '@playwright/test';

test.describe('DateTimeInput - Variants and Data Binding', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/app');
    await page.waitForLoadState('networkidle');

    // Navigate to DateTimeInput app
    const searchInput = page.getByTestId('sidebar-search');
    await expect(searchInput).toBeVisible();
    await searchInput.click();
    await searchInput.fill('date time input');
    await searchInput.press('Enter');

    const firstResult = page
      .locator('[data-sidebar="menu-item"], [data-sidebar="menu-sub-item"]')
      .filter({ hasText: /DateTimeInput/i })
      .first();

    await expect(firstResult).toBeVisible();
    await firstResult.click();
    await page.waitForLoadState('networkidle');
  });

  test('should display all variants and data binding inputs', async ({
    page,
  }) => {
    const appFrameElement = await page.waitForSelector(
      'iframe[src*="date-time-input"]'
    );
    const appFrame = await appFrameElement.contentFrame();
    expect(appFrame).not.toBeNull();
    if (!appFrame) throw new Error('App frame not found');

    // Check all main variant TestIds
    const mainTestIds = [
      'datetime-input-date-main',
      'datetime-input-date-disabled-main',
      'datetime-input-date-invalid-main',
      'datetime-input-date-nullable-main',
      'datetime-input-datetime-main',
      'datetime-input-datetime-disabled-main',
      'datetime-input-datetime-invalid-main',
      'datetime-input-datetime-nullable-main',
      'datetime-input-time-main',
      'datetime-input-time-disabled-main',
      'datetime-input-time-invalid-main',
      'datetime-input-time-nullable-main',
    ];
    for (const testId of mainTestIds) {
      await expect(appFrame.getByTestId(testId)).toBeVisible();
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
      await expect(appFrame.getByTestId(testId)).toBeVisible();
    }
  });

  test('should update value for Date variant', async ({ page }) => {
    const appFrameElement = await page.waitForSelector(
      'iframe[src*="date-time-input"]'
    );
    const appFrame = await appFrameElement.contentFrame();
    expect(appFrame).not.toBeNull();
    if (!appFrame) throw new Error('App frame not found');

    const dateInput = appFrame.getByTestId('datetime-input-date-main');
    await expect(dateInput).toBeVisible();
    await dateInput.click();
    // Pick the 15th of the current month
    const dayBtn = appFrame
      .locator('.rdp-day')
      .filter({ hasText: '15' })
      .first();
    await dayBtn.click();
    // Check that the value updated in the input
    await expect(dateInput).toContainText('-15');
  });

  test('should update value for DateTime variant', async ({ page }) => {
    const appFrameElement = await page.waitForSelector(
      'iframe[src*="date-time-input"]'
    );
    const appFrame = await appFrameElement.contentFrame();
    expect(appFrame).not.toBeNull();
    if (!appFrame) throw new Error('App frame not found');

    const dtInput = appFrame.getByTestId('datetime-input-datetime-main');
    await expect(dtInput).toBeVisible();
    await dtInput.click();
    // Pick the 10th of the current month
    const dayBtn = appFrame
      .locator('.rdp-day')
      .filter({ hasText: '10' })
      .first();
    await dayBtn.click();
    // Change the time
    const timeInput = appFrame.getByTestId('datetime-input-datetime-main-time');
    await timeInput.fill('12:34:56');
    // Check that the value updated in the input
    await expect(dtInput).toContainText('-10');
  });

  test('should update value for Time variant', async ({ page }) => {
    const appFrameElement = await page.waitForSelector(
      'iframe[src*="date-time-input"]'
    );
    const appFrame = await appFrameElement.contentFrame();
    expect(appFrame).not.toBeNull();
    if (!appFrame) throw new Error('App frame not found');

    const timeInput = appFrame.getByTestId('datetime-input-time-main');
    await expect(timeInput).toBeVisible();
    // Change the time
    const input = timeInput.locator('input[type="time"]');
    await input.fill('08:00:00');
    // Check that the value updated in the input (should show 08:00)
    await expect(input).toHaveValue('08:00:00');
  });

  test('should update value for nullable DateTime', async ({ page }) => {
    const appFrameElement = await page.waitForSelector(
      'iframe[src*="date-time-input"]'
    );
    const appFrame = await appFrameElement.contentFrame();
    expect(appFrame).not.toBeNull();
    if (!appFrame) throw new Error('App frame not found');

    const nullableInput = appFrame.getByTestId(
      'datetime-input-datetime-nullable-main'
    );
    await expect(nullableInput).toBeVisible();
    await nullableInput.click();
    // Pick the 20th of the current month
    const dayBtn = appFrame
      .locator('.rdp-day')
      .filter({ hasText: '20' })
      .first();
    await dayBtn.click();
    // Check that the value updated in the input
    await expect(nullableInput).toContainText('-20');
  });

  test('should reflect data binding for DateTime', async ({ page }) => {
    const appFrameElement = await page.waitForSelector(
      'iframe[src*="date-time-input"]'
    );
    const appFrame = await appFrameElement.contentFrame();
    expect(appFrame).not.toBeNull();
    if (!appFrame) throw new Error('App frame not found');

    const dtInput = appFrame.getByTestId('datetime-input-datetime-binding');
    await expect(dtInput).toBeVisible();
    await dtInput.click();
    // Pick the 25th of the current month
    const dayBtn = appFrame
      .locator('.rdp-day')
      .filter({ hasText: '25' })
      .first();
    await dayBtn.click();
    // Check that the value cell updates
    const valueCell = appFrame.locator('text=25').first();
    await expect(valueCell).toBeVisible();
  });
});
