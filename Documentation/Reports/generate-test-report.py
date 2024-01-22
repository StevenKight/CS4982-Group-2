import subprocess
import glob
import os


def generate_xml_reports(test_projects: [dict]) -> None:
    """
    Generate XML reports for all test projects
    :param test_projects: list of test projects
        - Each test project is a dictionary with the following keys:
            - "project": name of test project
            - "results-file": name of XML results file
    :return: None
    """
    # Get directory of this file
    current_dir = os.path.dirname(os.path.realpath(__file__))
    relative_path_to_code = '\\..\\..\\CapstoneGroup2'
    relative_path_to_xml = '.\\XML'

    # Get absolute path to code
    code_path = os.path.realpath(current_dir + relative_path_to_code)
    xml_path = os.path.realpath(current_dir + relative_path_to_xml)

    # Go through each test project
    for test_project in test_projects:
        # Specify the commands to run
        generate_results = 'dotnet test "./%s" --logger "nunit;LogFilePath=%s/%s"' % (test_project["project"], xml_path, test_project["results-file"])

        # Run the command in the code directory
        subprocess.run(generate_results, shell=True, cwd=code_path)


def build_test_run_line(test_run_lines: [str]) -> str:
    """
    Build a single group test run line from a list of test run lines
    :param test_run_lines: list of test run lines
    """

    duration = 0
    test_case_count = 0
    total = 0
    passed = 0
    failed = 0
    inconclusive = 0
    skipped = 0
    for line in test_run_lines:
        duration += float(line.split("duration=")[1].split(" ")[0].strip('"'))
        test_case_count += int(line.split("testcasecount=")[1].split(" ")[0].strip('"'))
        total += int(line.split("total=")[1].split(" ")[0].strip('"'))
        passed += int(line.split("passed=")[1].split(" ")[0].strip('"'))
        failed += int(line.split("failed=")[1].split(" ")[0].strip('"'))
        inconclusive += int(line.split("inconclusive=")[1].split(" ")[0].strip('"'))
        skipped += int(line.split("skipped=")[1].split(" ")[0].strip('"'))

    duration = str(round(duration, 6))

    passed_test = "Passed" if failed == 0 else "Failed" if skipped == 0 else "Inconclusive"

    start_time = test_run_lines[0].split("start-time=")[1].split("\" ")[0].strip('"')
    end_time = test_run_lines[-1].split("end-time=")[1].split("\">")[0].strip('">')
    full_run = '<test-run id="2" duration="%s" testcasecount="%d" total="%d" passed="%d" failed="%d" inconclusive="%d" skipped="%d" result="%s" start-time="%s" end-time="%s">'
    full_run = full_run % (duration, test_case_count, total, passed, failed, inconclusive, skipped, passed_test, start_time, end_time)

    return full_run


def combine_xml_reports(reports: [str]) -> None:
    """
    Combine multiple XML reports into one XML report
    :param reports: list of XML report file paths
    :return: None
    """

    combined_reports = ""

    for report_file in reports:
        with open(report_file, "r") as report:
            combined_reports += report.read() + "\n"
            
    test_run_lines = []
    for line in combined_reports.splitlines():
        if "<test-run " in line:
            test_run_lines.append(line)

    combined_report = build_test_run_line(test_run_lines) + "\n"

    for line in combined_reports.splitlines():
        combined_report += "\t" + line + "\n"

    combined_report += "</test-run>"

    with open("combined-test-results.xml", "w") as combined_report_file:
        combined_report_file.write(combined_report)
    

if __name__ == "__main__":
    # Setup all test projects
    test_projects = [
        {"project": "API Tests", "results-file": "api-test-results.xml"},
        {"project": "Desktop Client Tests", "results-file": "desktop-client-test-results.xml"}
    ]

    # Generate XML reports
    generate_xml_reports(test_projects)

    # Get all XML reports
    xml_reports = glob.glob("XML/*.xml")

    # Combine XML reports
    combine_xml_reports(xml_reports)
