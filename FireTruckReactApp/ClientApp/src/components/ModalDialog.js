import { useState } from 'react';

// Modal content component
function ModalContent(props) {
    const { data } = props;

    return (
        <div>
            <h2>{data.Identifier}</h2>
            <p>{data.email}</p>
            <p>{data.phone}</p>
        </div>
    );
}

// Main component
function ModalDialog() {
    const [showModal, setShowModal] = useState(false);
    const [selectedData, setSelectedData] = useState(null);

    const handleItemClick = (data) => {
        setSelectedData(data);
        setShowModal(true);
    }

    return (
        <div>
            <ul>
                {selectedData.map((data) => (
                    <li key={data.id} onClick={() => handleItemClick(data)}>
                        {data.name}
                    </li>
                ))}
            </ul>

            {showModal && (
                <div className="modal-overlay">
                    <div className="modal-content">
                        <button onClick={() => setShowModal(false)}>Close</button>
                        <ModalContent data={selectedData} />
                    </div>
                </div>
            )}
        </div>
    );
}
