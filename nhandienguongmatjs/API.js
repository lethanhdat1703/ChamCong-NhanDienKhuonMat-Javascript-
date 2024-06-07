async function fetchEmployeeData() {
  try {
    const response = await fetch("https://localhost:7286/api/employee", {
      method: "GET",
      mode: "cors",
      headers: {
        "Content-Type": "application/json",
        "X-PINGOTHER": "pingpong",
      },
    });
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error fetching employee data:", error);
    throw error;
  }
}

async function CheckIn(idEmpl, checkInDate) {
  try {
    const response = await fetch(
      `https://localhost:7286/api/Timekeeping/${idEmpl}`,
      {
        method: "POST",
        mode: "cors",
        headers: {
          "Content-Type": "application/json",
          "X-PINGOTHER": "pingpong",
        },
        body: JSON.stringify({ checkIn: checkInDate }),
      }
    );

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error fetching employee data:", error);
    throw error;
  }
}

async function CheckOut(idEmpl, checkOutDate) {
  try {
    const response = await fetch(
      `https://localhost:7286/api/Timekeeping/${idEmpl}`,
      {
        method: "Put",
        mode: "cors",
        headers: {
          "Content-Type": "application/json",
          "X-PINGOTHER": "pingpong",
        },
        body: JSON.stringify({ checkOut: checkOutDate }),
      }
    );

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error fetching employee data:", error);
    throw error;
  }
}
