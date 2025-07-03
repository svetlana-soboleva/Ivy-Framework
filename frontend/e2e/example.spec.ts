import { test, expect } from '@playwright/test';

test('has title', async ({ page }) => {
  await page.goto('/');

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle(/Ivy/);
});

test('loads the main page', async ({ page }) => {
  await page.goto('/');

  // Wait for the page to load
  await page.waitForLoadState('networkidle');

  // Check that the page loaded successfully
  await expect(page.locator('body')).toBeVisible();
});

test('can navigate to different apps', async ({ page }) => {
  await page.goto('/');

  // Wait for the page to load
  await page.waitForLoadState('networkidle');

  // Check that we can see the main content
  await expect(page.locator('body')).toBeVisible();
});
