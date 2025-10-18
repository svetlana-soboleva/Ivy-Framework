# Frontend

**Node.js Version Requirement**: This project requires Node.js version 22.12.0 or greater.

## Development

```bash
npm run dev
npm run build
```

## Developer Logging

The frontend includes a comprehensive logging system for debugging and development purposes. Detailed logging can be controlled via browser console commands.

### Console Commands

Open the browser console (F12 → Console tab) and use these commands:

```javascript
// Check current developer options
getDeveloperOptions();
// Returns: { showDetailedLogging: false }

// Toggle detailed logging on/off
toggleDeveloperLogging();
// Returns: true (if enabled) or false (if disabled)
// Also logs: "Developer logging enabled" or "Developer logging disabled"
```

### What Gets Logged

When detailed logging is enabled, you'll see debug messages for:

- **Select Input Interactions**: Value changes, conversions, clear operations
- **SignalR Communication**: Message processing, updates, events
- **Widget Tree Operations**: XML conversion, patches, updates
- **Authentication**: JWT operations, theme changes
- **Error Handling**: Connection issues, parsing errors

### Log Levels

- **Debug**: Detailed information (controlled by `showDetailedLogging`)
- **Info**: General information (always visible)
- **Warn**: Warning messages (always visible)
- **Error**: Error messages (always visible)

### Persistence

Developer options are stored in localStorage and persist across:

- Page refreshes
- Browser sessions
- Browser restarts

## Code Quality

The frontend project uses ESLint and Prettier for code quality and formatting, with automatic pre-commit hooks. It is also responsible for handling `dotnet format` precommit hook for the BE.

### Pre-commit Hooks

We use a Husky npm package to setup precommit hooks for both the FE and the BE.

To get the auto-linting for staged files, you need to have run `npm run install` in `./frontend` at least once. Ideally, you would not then need to run any formatting or lint commands as it will be done for you. In case you want to manually run them, you still can.

If you have Prettier and ESLint, you can configure your IDE to respect the repositories styling guidelines and easily format your code.

If there are issues that auto-linting and formatting can't be resolved, your commit will be blocked from being pushed. If you really need to push, you can specify checks behavior per commit (not recommended):

```bash
git commit --no-verify -m "Commit message"
```

### Code Formatting

Format all files with Prettier:

```bash
npm run format
```

Check if files are properly formatted:

```bash
npm run format:check
```

### Linting

Check for linting issues:

```bash
npm run lint
```

Automatically fix linting issues:

```bash
npm run lint:fix
```

### Configuration Files

- `.prettierrc` - Prettier configuration
- `.prettierignore` - Files to exclude from formatting
- `eslint.config.js` - ESLint configuration with Prettier integration
- `package.json` - Contains lint-staged configuration and scripts

## Testing

This project uses Vitest for unit testing and Playwright for end-to-end testing.

### Unit Testing with Vitest

Run unit tests:

```bash
npm run test
```

Unit tests are configured to run only on files ending with `.test.ts`. Place your unit test files alongside your source code with the `.test.ts` extension.

### End-to-End Testing with Playwright

### Prerequisites

Make sure you're in the frontend directory:

```bash
cd frontend
```

### Install Dependencies

```bash
npm ci
```

### Install Playwright Browsers

```bash
npx playwright install --with-deps
```

### Running Tests

Run all e2e tests:

```bash
npm run e2e
```

Run only Ivy.Docs e2e tests:

```bash
npm run e2e:docs
```

Run only Ivy.Samples e2e tests:

```bash
npm run e2e:samples
```

Run tests in a specific browser:

```bash
npm run e2e -- --project=chromium
npm run e2e -- --project=firefox
npm run e2e -- --project=webkit
```

Run tests in headed mode (to see the browser):

```bash
npm run e2e -- --headed
```

Run tests in debug mode:

```bash
npm run e2e -- --debug
```

Run a specific test file:

```bash
npm run e2e -- example.spec.ts
```

**Note**: We use npm scripts instead of `npx playwright test` to ensure consistent usage of the locally installed Playwright version and avoid version conflicts.

### Test Reports

View the HTML test report:

```bash
npm run e2e -- --reporter=html
# Then open the report
npx playwright show-report
```

### Test Files

- `e2e/` - End-to-end test files

### CI/CD

Tests are automatically run in GitHub Actions on push to main/master branches and pull requests. The CI pipeline includes:

1. Code formatting checks (`npm run format:check`)
2. Linting checks (`npm run lint`)
3. Unit tests (`npm run test`)
4. Playwright end-to-end tests

## Available Scripts

| Script                 | Description                           |
| ---------------------- | ------------------------------------- |
| `npm run dev`          | Start development server              |
| `npm run build`        | Build for production                  |
| `npm run preview`      | Preview production build              |
| `npm run test`         | Run unit tests with Vitest            |
| `npm run e2e`          | Run all end-to-end tests              |
| `npm run e2e:docs`     | Run Ivy.Docs end-to-end tests         |
| `npm run e2e:samples`  | Run Ivy.Samples end-to-end tests      |
| `npm run lint`         | Check for linting issues              |
| `npm run lint:fix`     | Fix linting issues automatically      |
| `npm run format`       | Format all files with Prettier        |
| `npm run format:check` | Check if files are properly formatted |
