import React, {useEffect, useState} from 'react';
import {Button, Modal} from 'react-bootstrap';
import {toast, ToastContainer} from 'react-toastify';
import FireTruckCard from "./components/FireTruckCard";
import LocationCard from "./components/LocationCard";
import LocationItemsCard from "./components/LocationItemsCard";
import 'react-toastify/dist/ReactToastify.css';
import "./App.css";

const locationData = [
    {"location": "G1"},
    {"location": "G2"},
    {"location": "G3"}
];

function App() {

    const [loading, setLoading] = useState(true);
    const [selectedTruck, setSelectedTruck] = useState([]);
    const [selectedLocation, setSelectedLocation] = useState([]);
    const [selectedLocationItem, setSelectedLocationItem] = useState([]);
    const [locationData, setLocationData] = useState([]);
    const [fireTruckData, setFireTruckData] = useState([]);
    const [locationItemsData, setLocationItemsData] = useState([]);
    const [itemData, setItemData] = useState([]);
    const [showModal, setShowModal] = useState(false);

    const handleCloseModal = () => setShowModal(false);
    const handleShowModal = () => {
        setShowModal(true);
    }

    const handleClickTruck = async (truck) => {

        console.log('Fire truck data:', truck);
        setSelectedTruck(truck)
        setLocationData(await fetchLocationData(truck.identifier));
    };
    const handleClickLocation = async (locationObject) => {
        console.log('Location data:', locationObject);
        setSelectedLocation(locationObject);
        setLocationItemsData(await fetchLocationItemsData(selectedTruck.identifier, locationObject.identifier));
    };

    async function fetchLocationItemsData(truck, identifier) {
        console.info("fetchLocationItemsData " + truck + " " + identifier)
        try {
            const response = await fetch(`${process.env.REACT_APP_API_HOST}FireTruckItems?firetruck=${truck}&location=${identifier}`);
            return await response.json();
        } catch (error) {
            toast.error(`Failed to fetch location items for truck ${identifier} data: ${error.message}`);
            throw error;
        }
    }

    const handleClickLocationItem = (id) => {
        console.log('Location item data:', id);
        setSelectedLocationItem(id);
    };

    useEffect(() => {

        fetchBasicData();
    }, []);

    const fetchBasicData = async () => {
        console.log('fetchBasicData:');
        console.info(`TEST ${process.env.REACT_APP_API_HOST}`)
        const data = await fetchFireTruckData();
        if (data) {
            setFireTruckData(data);
            setLoading(false);
        }
    }

    function handleRetry() {
        fetchBasicData().then(console.log("retry"), console.error("missed"))
        toast.info("Retry retrieving data")
    }

    const renderTrucks = () => {
        return (
            <div>
                {loading ? (
                    <div>
                        <p>Loading...</p>
                        <button onClick={handleRetry}>Retry</button>
                    </div>
                ) : (
                    <ul className="scrollable-horizontal-list ">
                        {
                            fireTruckData.map((item) => (
                                <FireTruckCard
                                    key={item.identifier}
                                    fireTruckData={item}
                                    selected={selectedLocation.identifier === item.identifier}
                                    handleClickTruck={() => {
                                        toast.success(`Select Trucks ${item.identifier}`)
                                        handleClickTruck(item)
                                    }
                                    }
                                />
                            ))
                        }
                    </ul>
                )}
            </div>
        );
    };

    const renderLocationItems = (id) => {

        console.log(`render location for location ${id}`)
        locationItemsData.forEach(x => console.warn(x))
        return locationItemsData.map((item) => (
            <LocationItemsCard
                key={item.identifier}
                locationItemsData={item}
                selected={selectedLocationItem.identifier === item.identifier}
                handleClick={() => {
                    toast.success(`Select location item  ${item.identifier} ${selectedLocationItem}`)
                    handleClickLocationItem(item.identifier)
                    handleShowModal()
                }
                }
            />
        ));
    };

    async function fetchLocationData(fireTruck) {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_HOST}FireTruckLocation?fireTruck=${fireTruck}`);
            return await response.json();
        } catch (error) {
            toast.error(`Failed to fetch location truck data: ${error.message}`);
            throw error;
        }
    }


    async function fetchFireTruckData() {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_HOST}FireTruck`);
            return await response.json();
        } catch (error) {
            toast.error(`Failed to fetch fire truck data: ${error.message}`);
            throw error;
        }
    }

    const renderLocations = (id) => {
        console.log(`render location for truck ${id}`)
        return locationData.map((item) => (
            <LocationCard
                key={item.identifier}
                locationData={item}
                truckData={selectedTruck}
                className={`card ${selectedLocation === item.identifier ? 'selected' : ''}`}
                selected={selectedLocation.identifier === item.identifier}
                handleClickLocation={() => {
                    toast.success(`Select location  ${item.identifier}`)
                    handleClickLocation(item)
                }
                }
            />
        ));
    };

    function ModalDialog({item}) {
        return (
            <div className="modal">
                <div className="modal-content">
                    <h2>{item.identifier}</h2>
                    <p>Age: {item.identifier}</p>
                    <button>Close</button>
                </div>
            </div>
        );
    }

    // position={"top-center"}
    return (
        <div className="container">
            <Modal show={showModal} scrollable={true} fullscreen={"xl-down"} onHide={handleCloseModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Modal Title</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>Modal content goes here...</p>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseModal}>
                        Close
                    </Button>
                </Modal.Footer>
            </Modal>
            <ToastContainer autoClose={3000} hideProgressBar={true} pauseOnHover={true} newestOnTop={true} limit={3}/>
            <div className="scrollable-horizontal-list">{renderTrucks()}</div>
            <div className="selectedTruckIdentifier">{selectedTruck.identifier}</div>
            <div className="scrollable-horizontal-list">{renderLocations(selectedTruck.identifier)}</div>
            <div className="scrollable-vertical-list">{renderLocationItems(selectedLocation.identifier)}</div>
            <div className="selectedTruckIdentifier">{selectedLocationItem.identifier}</div>
        </div>
    );
}

export default App;
